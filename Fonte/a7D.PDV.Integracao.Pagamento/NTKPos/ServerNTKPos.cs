using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Linq;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;
using System.IO;

namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public delegate void TerminalEvent(TerminalInformation terminal, string status);
    public delegate void RegErro(TerminalInformation terminal, Exception ex);

    public sealed class ServerNTKPos<T> : IDisposable where T : ITerminalClient, new()
    {
        #region singleton

        private static ServerNTKPos<T> instance = null;

        private ServerNTKPos()
        {
        }

        public void Dispose()
        {
            try
            {
                Running = false;
                Thread.Sleep(2 * Const.sleepTime); // Para tentar finalizar sem dar abort

                if (monitorLoop != null && monitorLoop.ThreadState != ThreadState.Stopped)
                    monitorLoop.Abort();

                for (int i = 0; i < terminals.Count; i++)
                {
                    var term = terminals[i];
                    if (term.thread != null && term.thread.ThreadState != ThreadState.Stopped)
                        term.thread.Abort();
                }

                terminals = null;
                monitorLoop = null;
            }
            catch (Exception)
            { }

            try
            {
                ClassPOSPGW.PTI_End();
            }
            catch (Exception)
            { }
        }

        public static ServerNTKPos<T> Start(Int16 port, TerminalEvent onEvent, RegErro onErro, bool runThread)
        {
            string version = Assembly.GetCallingAssembly().GetName().Version.ToString();

            if (instance != null)
                instance.Dispose();

            instance = new ServerNTKPos<T>();
            instance.StartServer(version, port, onEvent, onErro, runThread);
            return instance;
        }

        public static void Stop()
        {
            instance.Dispose();
            instance = null;
        }

        public static void Event(TerminalInformation term, string info)
        {
            instance.onTerminalEvent(term, info);
        }

        public static bool IsRunning => instance == null ? false : instance.Running;

        #endregion

        private Thread monitorLoop;
        private List<TerminalInformation> terminals;
        private bool Running;
        private TerminalEvent onTerminalEvent;
        private RegErro onErroEvent;

        private void StartServer(string version, int port, TerminalEvent onEvent, RegErro onErro, bool runThread)
        {
            if (!Directory.Exists(Const.Path))
                Directory.CreateDirectory(Const.Path);

            terminals = new List<TerminalInformation>();

            short ret = 99;
            ClassPOSPGW.PTI_Init(Const.appCompany, version, "0", Const.Path, port, Const.maxTerminals, Const.waitMessage, Const.autoDisconect, ref ret);
            if (ret != 0)
                throw new Exception($"[{ret}] Erro ao inicializar: " + termRetText(ret));

            onTerminalEvent += onEvent;
            onErroEvent += onErro;

            Running = true;
            onTerminalEvent(null, "Servidor NTK ativo porta " + port);

            if (runThread)
            {
                monitorLoop = new Thread(MonitorLoop);
                monitorLoop.Start();
            }
        }

        public void MonitorLoop()
        {
            onTerminalEvent(null, "POS NTK Ativo");
            while (Running)
            {
                Thread.Sleep(Const.sleepTime);
                var novo = termCheckForConnection();

                if (novo == null)
                    continue;

                var term = terminals.FirstOrDefault(t => t.terminalId == novo.terminalId);
                if (term == null)
                    terminals.Add(term = novo);

                onTerminalEvent(novo, $"CONECTADO {term.serialNumber} {term.mac}");

                if (term.thread != null && term.thread.ThreadState != ThreadState.Stopped)
                    continue;

                var client = new T();
                client.Terminal = novo;
                client.Create(onTerminalEvent, onErroEvent);

                term.thread = new Thread(TerminalLoop);
                term.thread.Start(client);
            }
        }

        private void TerminalLoop(object tc)
        {
            try
            {
                T client = (T)tc;
                var terminal = client.Terminal;
                bool connected = true;
                int status;
                while (IsRunning && client.Valid)
                {
                    status = termCheckStatus(ref terminal);
                    if (status == -1)
                    {
                        if (connected)
                        {
                            connected = false;
                            onTerminalEvent(terminal, "desconectado");
                        }
                        Thread.Sleep(Const.sleepTime);
                        continue;
                    }
                    if (!connected)
                    {
                        connected = true;
                        onTerminalEvent(terminal, "reconectou");
                    }
                    try
                    {
                        if (status == 0) // PTISTAT_IDLE
                            client.Loop();
                        else if (status == 1) // PTISTAT_BUSY
                            Thread.Sleep(200);
                    }
                    catch (Exception ex)
                    {
                        terminal.toAtual = terminal.toAnterior = terminal.toScreen = null;
                        onErroEvent(terminal, ex);
                    }
                }
                termDisconnect(terminal.terminalId, 0);
            }
            catch (Exception ex)
            {
                onErroEvent(null, ex);
            }
        }
    }
}

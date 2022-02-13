using a7D.PDV.Balanca.Interfaces;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace a7D.PDV.Balanca
{
    public abstract class Balanca : IBalanca, IDisposable
    {
        public string Porta { get; set; }
        internal Configuracao conf;
        protected SerialPort serialPort;
        private Dados dados = null;
        private CancellationTokenSource cSource;

        internal Balanca()
        {
            dados = new Dados("Timeout", null, Status.ERRO, -1);
        }

        private void NovaSerial()
        {
            serialPort = new SerialPort(Porta, conf.BaudRate, conf.Paridade, conf.DataBits, conf.BitsDeParada);
            serialPort.ReadTimeout = conf.TimeoutLeitura;
            serialPort.WriteTimeout = conf.TimeoutEscrita;
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string existing = sp.ReadExisting();
            if (!string.IsNullOrWhiteSpace(existing))
            {
                Dispose();
                dados = InterpretarDados(existing);
            }
            cSource.Cancel();
        }

        protected abstract Dados InterpretarDados(string existing);

        public async Task<Dados> LerPesoAsync()
        {
            try
            {
                NovaSerial();
                serialPort.ReceivedBytesThreshold = 7;
                serialPort.Open();
                if (!string.IsNullOrWhiteSpace(conf.Comando))
                    serialPort.WriteLine(conf.Comando);
                cSource = new CancellationTokenSource();

                await Task.Delay(conf.TimeoutLeitura + conf.TimeoutEscrita, cSource.Token).ConfigureAwait(false);
            }
            catch (TaskCanceledException) { }
            catch (Exception e)
            {
                dados = new Dados(e.Message, null, Status.ERRO, -1);
            }
            finally
            {
                Dispose();
            }
            return dados;
        }

        //public Dados LerPesoWs(string enderecoWs)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var uri = new Uri($"{enderecoWs}/{conf.Tipo.ToString().ToLower()}");

        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //            var response = client.GetAsync(uri).Result;
        //            var content = response.Content.ReadAsStringAsync().Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                dynamic body = JObject.Parse(content);
        //                var dados = new Dados(body.Conteudo.Value, ASCIIEncoding.ASCII.GetBytes(body.Ascii.Value), (Status)Convert.ToInt32(body.Status.Value), Convert.ToDecimal(body.Peso.Value, CultureInfo.InvariantCulture));
        //                return dados;
        //            }
        //            throw new Exception($"Erro ao obter peso pelo webservice: {content}");
        //        }
        //    }
        //    catch (TaskCanceledException) { }
        //    catch (Exception e)
        //    {
        //        dados = new Dados(e.Message, null, Status.ERRO, -1);
        //    }
        //    finally
        //    {
        //        Dispose();
        //    }
        //    return dados;
        //}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (serialPort?.IsOpen == true)
                        serialPort?.Close();
                    serialPort?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                serialPort = null;
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Balanca() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

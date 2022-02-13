using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#produtos
    public class Product : ModelCake
    {
        public string code; // Codigo ID PDV7
        public int? category; // Identificador da categoria do produto
        public int? supplier; // Identificador do fornecedor
        public string name; // Nome do produto
        public Decimal? price_sell; // Preço de Venda
        public Decimal? price_cost; // Preço de custo
        public Decimal? price_purchase; // Preço de comra
        public Decimal? stock; // Estoque
        public string barcode_ean; // Código de barras EAN
        public int? measure_unit; // Identificador da unidade de medida
        public int? brand; // Identificador da marca do produto
        public int? ncm; // Identificador do NCM do produto
        public Decimal? net_weight; // Peso líquido do produto
        public Decimal? gross_weight; // Peso bruto do produto
        public int? grid; // Identificador da grade
        public int? tax_purchase_id; // Identificador da tributação de compra
        public int? tax_sale_id; // Identificador da tributação de venda
        public int? tax_warranty_id; // Identificador da tributação de garantia
        public int? tax_devolution_id; // Identificador da tributação de devolução
        public int? code_number; // Código do produto
        public string round_type; // Tipo de arredondamento
        public string production_type; // Tipo de produção
        public bool? active; // Flag de ativo
        public string image; // Nome da imagem do produto
        public Decimal? markup; // Markup do produto
        public string technical_specifications; // Especificações Técnicas
        public string observation; // Observações
        public string model; // Modelo
        public Decimal? icms; // Percentual de ICMS de compra
        public Decimal? icms_st; // Percentual de ICMSST de compra
        public Decimal? ipi; // Percentual de IPI de compra
        public Decimal? shipping; // Percentual de frete de compra
        public Decimal? minimum_stock; // Estoque Mínimo
        public Decimal? maximum_stock; // Estoque Máximo
        public int? tax_id; // Identificador da tributação
        public string youtube_code; // Código do youtube
        public int? ind_ecommerce; // Flag de integração com e-commerce
        public int? product_type; // Tipo do Produto
        public bool? down_stock; // Flag de movimenta estoque
        public string supplier_ref_code; // Código de referência do fornecedor
        public string code_cest; // Código CEST
        public int? code_isbn; // Código ISBN
        public Decimal? weight_with_package; // Peso com Embalagem
        public Decimal? height; // Altura
        public Decimal? height_with_package; // Altura com embalagem
        public Decimal? width; // Largura
        public Decimal? width_with_package; // Largura com embalagem
        public Decimal? depth; // Profundidade
        public Decimal? depth_with_package; // Profundidade com embalagem
        public Decimal? loss; // Perda do produto
        public string production_observation; // Observação de produção
        public string localization; // Localização do produto no estoque
        public int? collection; // Identificador da coleção
        public Decimal? price_promo; // Preço promocional
        public bool? use_grid; // Flag de produto de grade
        public DateTime? creation_time; // Data e hora de criação do produto
        public DateTime? update_time; // Data e hora de alteração do produto

        public override bool RequerAlteracaoPDV(DateTime dtSync, out int id)
        {
            if (string.IsNullOrEmpty(code))
            {
                var prod = EF.Repositorio.Carregar<EF.Models.tbProduto>(p => p.Nome == name);
                if (prod != null)
                {
                    if (!string.IsNullOrEmpty(prod.CodigoERP)
                        && int.Parse(prod.CodigoERP) == this.id)
                    {
                        id = prod.IDProduto;
                        return true;
                    }
                    else
                    {
                        id = 0;
                        return false;
                    }
                }
                id = 0;
                return true;
            }
            else
            {
                // Se tiver erro precisa mostrar em tela, para evitar códigos alfanumericos!!!!
                id = int.Parse(code);
                return update_time > dtSync;
            }
        }

        public override void SetCode(string code) => this.code = code;

        public override string ToString() => $"{id}({code}): {name}";
    }

    public class CodigoERP_QTD_Nome
    {
        public string CodigoERP { get; set; }
        public int QTD { get; set; }
        public string Nome { get; set; }
    }
}
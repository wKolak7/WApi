using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using WAppi.Extensions;
using WAppi.Models;
using WAppi.Services.Interfaces;

namespace WAppi.Services
{
    public class P24Service : IP24Service
    {
        private readonly IConfiguration _configuration;
        private readonly List<Order> testOrders = new List<Order>();

        public P24Service(IConfiguration configuration)
        {
            _configuration = configuration;
            testOrders.Add(new Order());
        }

        public async Task<Dictionary<string, string>> RegisterTransactionAsync(Order order)
        {
            //Get current configuration
            var p24_crc = new P24Extensions(_configuration).GetP24Crc();
            var p24_merchant_id = _configuration["p24_merchant_id"];
            var p24_pos_id = _configuration["p24_pos_id"];
            var p24_api_version = _configuration["p24_api_version"];
            var p24_channel = _configuration["p24_channel"];
            var p24_currency = _configuration["p24_currency"];

            //Generate sign
            var crc = p24_crc;
            var pos = p24_merchant_id;
            string hash = "";
            using (MD5 md5Hash = MD5.Create())
            {
                string str = $"{order.Id}|{p24_merchant_id}|{Convert.ToInt32(order.SummaryPrice * 100)}|{p24_currency}|{crc}";
                hash = CryptographicExtenstions.GetMd5Hash(md5Hash, str);
            }

            //Set up transaction
            P24Product p = new P24Product();
            p.P24_name_ = $"{order.ProductName} - {order.Amount} X";
            p.P24_description_ = "";
            p.P24_quantity_ = 1;
            p.P24_price_ = Convert.ToInt32(order.SummaryPrice * 100);

            var products = new List<P24Product>();
            products.Add(p);

            var backLink = $"https://localhost:5000/moje-konto/zamowienia/{order.Id}";
            var t = new P24Transaction(int.Parse(p24_merchant_id), int.Parse(p24_pos_id), order.Id.ToString(), order.Amount, p24_currency, $"Zamówienie: {order.OrderNumber} ", order.ClientEmail, $"{order.ClientName} {order.ClientLastName}", "", "", "", "", "", "pl", 16, backLink, "https://localhost:5000/api/Callback/p24", 0, 1, int.Parse(p24_channel), 100, $"Zamówienie: {order.OrderNumber}", p24_api_version, hash, "UTF-8", products);
            
            using (var client = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("p24_merchant_id", t.P24_merchant_id.ToString()),
                    new KeyValuePair<string, string>("p24_pos_id", t.P24_pos_id.ToString()),
                    new KeyValuePair<string, string>("p24_session_id", t.P24_session_id),
                    new KeyValuePair<string, string>("p24_amount", t.P24_amount.ToString()),
                    new KeyValuePair<string, string>("p24_currency", t.P24_currency),
                    new KeyValuePair<string, string>("p24_description", t.P24_description),
                    new KeyValuePair<string, string>("p24_email", t.P24_email),
                    new KeyValuePair<string, string>("p24_client", t.P24_client),
                    new KeyValuePair<string, string>("p24_address", t.P24_address),
                    new KeyValuePair<string, string>("p24_zip", t.P24_zip),
                    new KeyValuePair<string, string>("p24_city", t.P24_city),
                    new KeyValuePair<string, string>("p24_country", t.P24_country),
                    new KeyValuePair<string, string>("p24_language", t.P24_language),
                    new KeyValuePair<string, string>("p24_url_return", t.P24_url_return),
                    new KeyValuePair<string, string>("p24_url_status", t.P24_url_status),
                    new KeyValuePair<string, string>("p24_time_limit", t.P24_time_limit.ToString()),
                    new KeyValuePair<string, string>("p24_wait_for_result", t.P24_wait_for_result.ToString()),
                    new KeyValuePair<string, string>("p24_channel", t.P24_channel.ToString()),
                    new KeyValuePair<string, string>("p24_shipping", t.P24_shipping.ToString()),
                    new KeyValuePair<string, string>("p24_transfer_label", t.P24_transfer_label.ToString()),
                    new KeyValuePair<string, string>("p24_api_version", t.P24_api_version.ToString()),
                    new KeyValuePair<string, string>("p24_encoding", t.P24_encoding),
                    new KeyValuePair<string, string>("p24_transfer_label", t.P24_pos_id.ToString()),
                    new KeyValuePair<string, string>("p24_name_1", $"{t.P24_description}"),
                    new KeyValuePair<string, string>("p24_quantity_1", "1"),
                    new KeyValuePair<string, string>("p24_price_1", t.P24_amount.ToString()),
                    new KeyValuePair<string, string>("p24_sign", t.P24_sign)
                });

                var registerUrl = new P24Extensions(_configuration).GetP24RegisterUrl();

                var response = await client.PostAsync(registerUrl, formContent);
                var finalResponse = await response.Content.ReadAsStringAsync();
                var dict = HttpUtility.ParseQueryString(finalResponse);
                var dictAllKeys = dict.AllKeys.ToDictionary(k => k, k => dict[k]);

                return dictAllKeys;
            }
        }

        public async Task<Dictionary<string, string>> VerifyTransactionAsync(IFormCollection data)
        {

            var order = testOrders.FirstOrDefault(x=>x.Id == Guid.Parse(data["p24_session_id"]));

            if(order == null) throw new Exception($"Invalid order id: {data["p24_session_id"]}");

            var p24_crc = new P24Extensions(_configuration).GetP24Crc();
            var p24_merchant_id = _configuration["p24_merchant_id"];
            var p24_currency = _configuration["p24_currency"];

            using (var client = new HttpClient())
            {
                var crc = p24_crc;
                var pos = p24_merchant_id;
                string hash = "";
                using (MD5 md5Hash = MD5.Create())
                {
                    string str = $"{order.Id}|{data["p24_order_id"]}|{Convert.ToInt32(order.SummaryPrice * 100)}|{p24_currency}|{crc}";
                    hash = CryptographicExtenstions.GetMd5Hash(md5Hash, str);
                }
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("p24_merchant_id", _configuration["p24_merchant_id"]),
                    new KeyValuePair<string, string>("p24_pos_id", _configuration["p24_pos_id"]),
                    new KeyValuePair<string, string>("p24_session_id", order.Id.ToString()),
                    new KeyValuePair<string, string>("p24_amount", Convert.ToInt32(order.SummaryPrice*100).ToString()),
                    new KeyValuePair<string, string>("p24_currency", _configuration["p24_currency"]),
                    new KeyValuePair<string, string>("p24_order_id", data["p24_order_id"]),
                    new KeyValuePair<string, string>("p24_sign", hash)
                });
                var verifyUrl = new P24Extensions(_configuration).GetP24VerifyUrl();

                var response = await client.PostAsync(verifyUrl, formContent);
                var finalResponse = await response.Content.ReadAsStringAsync();

                var dict = HttpUtility.ParseQueryString(finalResponse);
                var dictKeys = dict.AllKeys.ToDictionary(k => k, k => dict[k]);

                if (dictKeys.FirstOrDefault(x => x.Key == "error").Value == "0")
                {
                    //success 
                    order.SetAsPaid();
                    
                }
                return dictKeys;
            }
        }

    }
}

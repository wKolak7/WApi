using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAppi.Models
{
    public class P24Transaction
    {
        public int P24_merchant_id { get; set; }
        public int P24_pos_id { get; set; }
        public string P24_session_id { get; set; }
        public int P24_amount { get; set; }
        public string P24_currency { get; set; }
        public string P24_description { get; set; }
        public string P24_email { get; set; }
        public string P24_client { get; set; }
        public string P24_address { get; set; }
        public string P24_zip { get; set; }
        public string P24_city { get; set; }
        public string P24_country { get; set; }
        public string P24_phone { get; set; }
        public string P24_language { get; set; }

        public int P24_method { get; set; }
        public string P24_url_return { get; set; }
        public string P24_url_status { get; set; }

        public int P24_time_limit { get; set; }
        public int P24_wait_for_result { get; set; }
        public int P24_channel { get; set; }

        public int P24_shipping { get; set; }
        public string P24_transfer_label { get; set; }
        public string P24_api_version { get; set; }
        public string P24_sign { get; set; }
        public string P24_encoding { get; set; }
        public List<P24Product> P24Products { get; set; }
        public P24Transaction(int p24_merchant_id, int p24_pos_id, string p24_session_id, int p24_amount, string p24_currency, string p24_description, string p24_email, string p24_client, string p24_address, string p24_zip, string p24_city, string p24_country, string p24_phone, string p24_language, int p24_method, string p24_url_return, string p24_url_status, int p24_time_limit, int p24_wait_for_result, int p24_channel, int p24_shipping, string p24_transfer_label, string p24_api_version, string p24_sign, string p24_encoding, List<P24Product> products)
        {
            P24_merchant_id = p24_merchant_id;
            P24_pos_id = p24_pos_id;
            P24_session_id = p24_session_id;
            P24_amount = p24_amount;
            P24_currency = p24_currency;
            P24_description = p24_description;
            P24_email = p24_email;
            P24_client = p24_client;
            P24_address = p24_address;
            P24_zip = p24_zip;
            P24_city = p24_city;
            P24_country = p24_country;
            P24_phone = p24_phone;
            P24_language = p24_language;

            P24_method = p24_method;
            P24_url_return = p24_url_return;
            P24_url_status = p24_url_status;

            P24_time_limit = p24_time_limit;
            P24_wait_for_result = p24_wait_for_result;
            P24_channel = p24_channel;

            P24_shipping = p24_shipping;
            P24_transfer_label = p24_transfer_label;
            P24_api_version = p24_api_version;
            P24_sign = p24_sign;
            P24_encoding = p24_encoding;

            P24Products = products;
        }
    }
}

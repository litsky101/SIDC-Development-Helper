using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIDCMySQLHelper;
using SIDC_Development_Helper;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var conn = new MySQLHelper("Server=localhost;Database=sidc_pigibig;Username=root;Password=supervisor"))
                {
                    conn.ArgMySQLCommand = new StringBuilder("SELECT * FROM pig_tran00 ORDER BY ID ASC");

                    using (var dr = conn.GetMySQLReader())
                    {
                        var test = new List<Pigibig>();

                        while (dr.Read())
                        {
                            test.Add(new Pigibig
                            {
                                ReferenceNo = dr["REFERENCE_NO"].ToString(),
                                Sow = dr["SOW_NO"].ToString(),
                                Parity = dr["PARITY_NO"].ToString()
                            });
                        }
                    }

                }
            }
            catch
            {

                throw;
            }
        }
    }

    class Pigibig
    {
        public string ReferenceNo { get; set; }
        public string Sow { get; set; }
        public string Parity { get; set; }
    }
}

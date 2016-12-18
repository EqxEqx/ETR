using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseT //.PageModels
{
    public static class Common
    {
        public static List<String> AccountListInit()
        {
            List<String> lst = new List<string>();

            lst.Add("Common");
            lst.Add("DC");
            lst.Add("SF");
            lst.Add("219");

            return (lst);
        }

        public static List<String> CategoryListInit()
        {
            List<String> lst = new List<string>();

            lst.Add("Mgt");
            lst.Add("Travel");
            lst.Add("Office");
            lst.Add("Other");

            return (lst);
        }

        public static List<String> Category2ListInit()
        {
            List<String> lst = new List<string>();

            lst.Add("Equipment");
            lst.Add("Gas");
            lst.Add("Lodging");
            lst.Add("Maint");
            lst.Add("Meal");
            lst.Add("Miles");
            lst.Add("Phone");
            lst.Add("Tolls");
            lst.Add("Other");

            return (lst);
        }


        public static bool createFileName(string sz, DateTime dt, string fType, out string fName)
        {
            fName = "";

            try
            {
                string szNoSpace = sz.Trim().Replace(' ', '_');

                fName = string.Format("{0}_{1}.{2}", CommonDate.Convert2DateString(dt, true), szNoSpace, fType);

                return (true);
            }
            catch
            {
                return (false);
            }
        }


    }

    public static class CommonMath
    {
        public static Boolean convert2Decimal( string sVal, int precision, out decimal decVal )
        {
            try
            {
                decVal = Math.Round(Convert.ToDecimal(sVal.Trim()), precision);
                return (true);
            }
            catch
            {
                decVal = 0.0m;
                return (false);
            }
        }
    }

    public static class CommonDate
    {
        /// <summary>
        /// byyyMMdd = true, returns 20160731
        /// byyyMMdd = false, returns 2016-07-31
        /// </summary>
        /// <param name="dtVal"></param>
        /// <param name="byyyMMdd"></param>
        /// <returns></returns>
        public static string Convert2DateString(DateTime dtVal, bool byyyMMdd)
        {
            try
            {
                string sz;

                if(byyyMMdd == true)
                    sz = dtVal.ToString("yyyyMMdd");
                else
                    sz = dtVal.ToString("yyyy-MM-dd");

                return (sz);
            }
            catch (Exception ex)
            {
                string eMsg = ex.Message;
                return ("1900-01-01");
            }
        }

        public static DateTime Convert2DateTime(string yyyyMMdd)
        {
            DateTime dtVal;
            try
            {
                dtVal = Convert.ToDateTime(yyyyMMdd);
                return (dtVal);
            }
            catch (Exception ex)
            {
                string eMsg = ex.Message;
                dtVal = Convert.ToDateTime("1900-01-01");
                return (dtVal);
            }
        }
    }
}

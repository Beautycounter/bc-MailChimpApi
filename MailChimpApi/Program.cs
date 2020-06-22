using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Newtonsoft.Json;

namespace MailChimpApi
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine(@"Begin MailChimp API");

            string APIKey = @"8e8f6cfc1072b07b7b2a7adfb49f2858-us7";
            string listId = @"2adcf5ce97"; //Master Consultant - ALL - New
            DateTime StartDate = DateTime.Parse("2020-06-01 00:00:00");

            var @MailChimpInterface = new _MailChimpInterface();

            

            DataTable dtResults = await @MailChimpInterface.getCampaignsAsync(APIKey, listId, StartDate);

            IMailChimpManager mailChimpManager = new MailChimpManager(APIKey);
            var Lists = await mailChimpManager.Lists.GetAllAsync().ConfigureAwait(false);

            foreach (var List in Lists)
            {
                Console.WriteLine(List.Id + '-' + List.Name);

                //try
                //{



                //    var listUsers = await mailChimpManager.Members.GetAllAsync(List.Id).ConfigureAwait(false);


                //    //foreach (var user in listUsers)
                //    //{

                //    //    Console.WriteLine(@"    " + user.EmailAddress);

                //    //    foreach (var tag in user.Tags)
                //    //    {
                //    //        Console.WriteLine(@"        " + tag.Name);


                //    //    }

                //    //}


                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}

            }

            Console.WriteLine(@"Done");
            Console.ReadLine();
        }
    }
}

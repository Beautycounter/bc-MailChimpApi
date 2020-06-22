using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Newtonsoft.Json;


namespace MailChimpApi
{
    class _MailChimpInterface
    {

        public async System.Threading.Tasks.Task<DataTable> getCampaignsAsync(string APIKey, string listId, DateTime StartDate)
        {

            DataTable dtResults = new DataTable();
            dtResults.Columns.Add(@"CampaignId", typeof(string));
            dtResults.Columns.Add(@"SendTime", typeof(string));
            dtResults.Columns.Add(@"Campaign", typeof(string));


            try
            {

                IMailChimpManager mailChimpManager = new MailChimpManager(APIKey);
                

                var options = new CampaignRequest
                {
                    ListId = listId,
                    Status = CampaignStatus.Sent,
                    SortOrder = CampaignSortOrder.ASC,
                    Limit = 1000,
                    SinceCreateTime = StartDate
                };

                var Campaigns = await mailChimpManager.Campaigns.GetAllAsync(options);

                foreach (var Campaign in Campaigns)
                {
                    Console.WriteLine(Campaign.Id);
                    Console.WriteLine(Campaign.SendTime);
                    var json = JsonConvert.SerializeObject(Campaign);
                    Console.WriteLine(json);

                    dtResults.Rows.Add(Campaign.Id, Campaign.SendTime, json);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(@"Error: " + e.Message);
            }

            return dtResults;
        }


    }
}

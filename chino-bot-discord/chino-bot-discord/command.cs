using Discord.Commands;
using System.Threading.Tasks;
using Codeplex.Data;
using System.Net;
using System.Text;

namespace chino_bot_discord
{
    public class Chino : ModuleBase
    {
        [Command("test")]
        public async Task test()
        {
            await ReplyAsync("ok");
        }
        [Command("おはよう")]
        public async Task GoodMorning()
        {
            await ReplyAsync("おはようございます");
        }
        [Command("天気")]
        public async Task Weather()
        {
            await ReplyAsync(GetWeatherText());
        }
        
        private static string GetWeatherText()
        {
            const string NO_VALUE = "---";

            var url = "http://weather.livedoor.com/forecast/webservice/json/v1?city=130010";
            var req = WebRequest.Create(url);

            using (var res = req.GetResponse())
            using (var s = res.GetResponseStream())
            {
                dynamic json = DynamicJson.Parse(s);

                //天気(今日)
                dynamic today = json.forecasts[0];

                string dateLabel = today.dateLabel;
                string date = today.date;
                string telop = today.telop;

                var sbTempMax = new StringBuilder();
                dynamic todayTemperatureMax = today.temperature.max;
                if (todayTemperatureMax != null)
                {
                    sbTempMax.AppendFormat("{0}℃", todayTemperatureMax.celsius);
                }
                else
                {
                    sbTempMax.Append(NO_VALUE);
                }

                var sbTempMin = new StringBuilder();
                dynamic todayTemperatureMin = today.temperature.min;
                if (todayTemperatureMin != null)
                {
                    sbTempMin.AppendFormat("{0}℃", todayTemperatureMin.celsius);
                }
                else
                {
                    sbTempMin.Append(NO_VALUE);
                }

                //天気概況文
                var situation = json.description.text;

                //Copyright
                var link = json.copyright.link;
                var title = json.copyright.title;

                return string.Format("{0}\n天気 {1}\n最高気温 {2}\n最低気温 {3}\n\n{4}\n\n{5}\n{6}",
                    date,
                    telop,
                    sbTempMax.ToString(),
                    sbTempMin.ToString(),
                    situation,
                    link,
                    title
                    );

            }
        }
    }

}

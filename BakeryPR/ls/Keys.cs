﻿using BakeryPR.Models;
using BakeryPR.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BakeryPR.ls
{
    public class Keys
    {
        public List<string> all
        {
            get
            {
                return new List<string>()
                {
                    "8D318426E5B3492FBD9327B5DD330B04",
"CA1634AF7A1140F8A644C5A0E51DE9D9",
"72560409FB844DBDB16FFCCF0566E817",
"7EBC8C69AF8B47BC95FD32DCC6CF94EC",
"C4D8DB4C6D234CE3B8E0148A1E65A85C",
"913DBA9B785A47DDAD7227A1387E637F",
"F2F0964C9CB647E185870561CEF8923E",
"16B4CA83726A4891B808D4C0FB4EDD2B",
"C2C69D4B2671453AAD33F6B398AD9361",
"3C59AC352C7E4BF09F791322240E1B1B",
"78BFD68EEE0245549BA973522866FCE1",
"EE269F0EDDB94DC483BA6399EC875BEF",
"E4CD9AC2FDB24EBCA29D1C7B01A2B71F",
"B9C10F252EC5487E82FAD2DB0F001AE5",
"B98345C580C74FF0A8ECDBB03E5D6895",
"A4B46109EA114CC88172AE92C56D233E",
"F5802CF9C8824D63BA22F430216E7E20",
"AA424C4F33674ABBA1B506F83F692443",
"CCAB2238BADA47038C0B686ADC8D3994",
"869A41C601C048AF9B3DA548A305BE24",
"B03EB8D113014612B7C5FF6586CAE107",
"C7689A05C2574ED89B30435C6F1ADF8D",
"1AE71DC1F3CE46B2941694247BB23ABE",
"B76737376FDD4CB4993C1F89B047A96E",
"DB1AE913B9964A8E99C82D3EDE540E72",
"ED0793A1A6AD474492BAC15D455A9162",
"6AC62C52EC4C44208529BC333FFBC5AB",
"13ACEDC2922442378BE39D09A683BC61",
"6A1C75AB81D8435D8FE3259423A9DC37",
"8C13DCCE5CE34A2D945A5F950572AC83",
"7020136CE8B3422EBAC36C8744640398",
"311363B0E2224DE0BB86CC8BE4AC4463",
"1A8CDDF6A8E442DFB53D997256BA276F",
"6B6E6402BAF249EFBD8ECB4C90717A63",
"B977B52705264BDBA0855B9F16BF5071",
"F6D00BE8E2F640599250EA11EDF1CF4A",
"5B2D707A7C644D06B171607466E07020",
"B08BDF10CC5F4F99A439E77A8F5D4D6B",
"6994E25524EF4502A883DD809746FD08",
"8E14853CA18C4680BE3378ED361DE557",
"F956A75F9D964877B8623F3B3E231BBE",
"386C7A7A82684898A008C1CD7C93817B",
"2B338636847F41D3866DF64FD3B87BDA",
"8AB76B2BC3B9418CB4A6486A49588067",
"39B743C2672F4DD798D55D83319E3D56",
"D1115B2A2E954AF79A2B54F9C00D5CE6",
"44D4AA3FA2BF493D97DE7552A9C2B1BC",
"B7EC9CE2D6754FF9A3BD2DA413D861D9",
"421EB4615A444D5E9B92036516D75AD9",
"5D2271BF7A5849029C7D4F729111FDE0",
"F5917DA7F013485ABCD8210E44F2D960",
"917CCDFF0E4645C5A01435EF3F8F9561",
"7121C422D16C41F6AB8C746F313255BD",
"34A79BBEFA114AD0AEB8CAB09D395494",
"2463338DE28F49DBBEF85D1D47395B91",
"D51D0129D6B247B3A7E91FB22C2A4FC9",
"28A1DF581AD0419FA176F58EF22F9776",
"92F9B3B498E24886B43492CEF13D2A9E",
"73070BD8174344DCA463CB47C66AFF60",
"C23ADED8A9C946F6BDACE6073A14810A",
"2122F396D1454BB998D7E2C67D132D98",
"C4F5684D20724410AA8446690D960EA4",
"E7B0A612DBBF46BC92989C12124A469C",
"BFF0CD174F1948388190DA2BF861850A",
"57057BDE21E4467A9FD227093EFC22DF",
"5557722470E64C33BC4807212A2D0139",
"9FBC7C6F6F5749149B83C3D9F38C031D",
"10E39EE3451F48339F462F036815AB51",
"E611580997044049B362C666AD859B33",
"A8978C918AFA4FBB96360F03E9162B80",
"13A556700D744A528662A2BABD054B10",
"280E9D3CD0934CE9AAD78E20970DE400",
"1D98D8BE92EE4F288EB25922E0818340",
"925BF579E0584D2FB679A912F42B7EA3",
"A14D17105E114B1D9142529D52C1F7C4",
"5BD6A1CE9AA4449B9DAF69DEA8FEAAAC",
"B4FE5DA5600B472788BC3DA42B57BF79",
"A4833E5298984EF895D89A08A013FADF",
"58DAD6CC51CE4E3F958300B3A1A684CE",
"39950AF4655E40198A55CD094A7CE18B",
"A142EC4E118643378EA7D71524BFEA57",
"A4002AD2C44948999430AF2F10E1B54B",
"4409009E2A62464EA3EB21B948559485",
"FFE96B8DC0724BB9B0E4C8F0E111FCB7",
"8B73A6FAFAA34552BDB2660691B9F922",
"FB511A3DB9254A1F8CDCCBADE9DBE34B",
"694E1594AB8544F392F6147F6F16B15D",
"085F6A8E54254E79B7B80BD4F5411CA0",
"93A11B3474214FF98A920C952D044B3A",
"B7EA3B30E4E646A58999E139A8061FAA",
"B9FE7F3A25C54068A0458298002A8E8D",
"5F7A8F9A9FF448D1AE07CA7076C0352E",
"B91073B827E4410F93B21243E5C8248A",
"9A078D97624244B6A0F8DFCBB729D0AA",
"E84AD93123774658ABBB5FA38A78F8D9",
"B2883EB87BDF4476BB5DCF515E9C4FAF",
"D87883EC1F4E49F79725B6425829FED9",
"8A4EE9324A9D4263BCF3B28DCE6946F7",
"3D3DB6A434A24CA8B5919B10D0F8C581",
"722A9549AD3E4518B41232BA0E44C258"
                };
            }
        }

        public string validate(string val)
        {
            return all.FirstOrDefault(x => getPass(x).ToLower() == val);
        }

        public async static Task<LicenseModel> getRemoteKeys(string key)
        {
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Accept.Clear();
                hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await hc.GetAsync("https://drive.google.com/uc?id=0B5YtDP-cRTqfOWpENkV0UTlWc00&export=download");
                string result = await response.Content.ReadAsStringAsync();
                string[] allkeys = result.Split(new char[] { ',' }).Select(x => x.Trim()).ToArray();
                LicenseModel keyModel = GetStructuredKeys(allkeys, key);
                if (keyModel == null)
                {
                    throw new Exception("Application could not validate Key");
                }
                return keyModel;
            }
            catch (Exception x)
            {
                throw new Exception("Application could not validate Key");
            }

        }

        public static LicenseModel GetStructuredKeys(string[] str, string key)
        {
            string appName = AppConfigUtils.Read("appCodeName").Substring(3, 6);
            LicenseModel lst = new LicenseModel();
            foreach (var tm in str)
            {
                String[] sd = tm.Split(new char[] { ':' });
                if (sd.Length >= 4)
                {
                    if (sd[1] == appName && key == getPass(sd[0]))
                    {
                        LicenseModel keyModel = new LicenseModel();
                        keyModel.appName = appName;
                        keyModel.appVersion = sd[2];
                        keyModel.key = sd[0];
                        keyModel.isUsed = sd[3] == "0" ? false : true;
                        keyModel.hostName = Environment.MachineName;
                        lst = keyModel;
                        break;
                    }
                    else
                    {
                        lst = null;
                    }
                }
            }
            return lst;
        }

        public static string getPass(string val)
        {
            string letters = "";
            int totalCount = 0;
            foreach (char tm in val.ToCharArray())
            {
                if (char.IsDigit(tm))
                {
                    totalCount += int.Parse(tm.ToString());
                }
                else
                {
                    letters += tm;
                }
            }
            return (letters.Length > 5 ? letters.Substring(1, 4) : letters) + totalCount;
        }

        public static bool ValidateKey(LicenseModel licenseModel,string appName)
        {
            byte[] byteValue = Convert.FromBase64String(licenseModel.value);
            String valueString = Encoding.UTF8.GetString(byteValue);
            LicenseModel m = JsonConvert.DeserializeObject<LicenseModel>(valueString);

            if (m.hostName == Environment.MachineName && AppConfigUtils.Read("key") == getPass(m.key)
                && appName.ToLower() == m.appName.ToLower())
            {
                return true;
            }

            return false;
        }

    }
}

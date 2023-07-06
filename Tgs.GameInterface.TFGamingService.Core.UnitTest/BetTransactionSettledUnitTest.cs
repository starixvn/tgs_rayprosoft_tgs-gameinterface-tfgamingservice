using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class BetTransactionSettledUnitTest
{
    [TestMethod]
    public async Task 查詢已結算注單()
    {
        var apiUrl = new Uri("https://spi-test.r4espt.com/api/v2/bet-transaction/");
        var privateToken = "0002fc0c96401e68419756f562689ed6e8e1ea398406f710d4d3f1c99b04f61b8fc0bea712b7d674f8be0dbb69a7a3aca0b02b423c73257eeb87521799ec7479218285597e314f5b18189eb8ebfc2bbf4dea799ad3b2fc74d99abb709ca86bc03d4daa5b17b078d4601152920903d18546c7335cb1008d2158f999ba0810a3643fcb";
        var startTime = DateTime.Parse("2022-12-23T05:43:37Z");
        var endTime = DateTime.Parse("2022-12-23T07:43:37Z");
        var pageSize = 100;

        var response = """
            {
                "count": 2,
                "next": null,
                "previous": null,
                "results": [
                    {
                        "id": "CCYCD185145H2900MGZHPZ494B",
                        "bet_selection": "attack",
                        "odds": 0.97,
                        "currency": "RMB",
                        "amount": 10.0,
                        "game_type_name": "DOTA 2 Attack",
                        "game_market_name": "Roll",
                        "market_option": "match",
                        "map_num": null,
                        "bet_type_name": "SPAD",
                        "competition_name": "LootBox",
                        "event_id": 12789000,
                        "event_name": "13751910",
                        "event_datetime": "2022-12-23T07:28:56.432111Z",
                        "date_created": "2022-12-23T07:29:01.892916Z",
                        "settlement_datetime": "2022-12-23T07:29:07.208033Z",
                        "modified_datetime": "2022-12-23T07:41:19.995275Z",
                        "settlement_status": "settled",
                        "result": "44",
                        "result_status": "WIN",
                        "earnings": 19.7,
                        "handicap": "50.0",
                        "is_combo": false,
                        "member_code": "4ucmqplxb029",
                        "is_unsettled": false,
                        "ticket_type": "live",
                        "malay_odds": 0.97,
                        "euro_odds": 1.97,
                        "member_odds": 0.97,
                        "member_odds_style": "hongkong",
                        "game_type_id": 39,
                        "request_source": "unknown"
                    },
                    {
                        "id": "CCYCD185145H2420MGFVMX4F01",
                        "bet_selection": "female",
                        "odds": 0.95,
                        "currency": "RMB",
                        "amount": 500.0,
                        "game_type_name": "KOG Draw (Fast)",
                        "game_market_name": "Gender",
                        "market_option": "match",
                        "map_num": null,
                        "bet_type_name": "SPXX",
                        "competition_name": "LootBox",
                        "event_id": 12788801,
                        "event_name": "13751713",
                        "event_datetime": "2022-12-23T07:24:14.867910Z",
                        "date_created": "2022-12-23T07:24:21.343061Z",
                        "settlement_datetime": "2022-12-23T07:24:30.257837Z",
                        "modified_datetime": "2022-12-23T07:33:44.682550Z",
                        "settlement_status": "settled",
                        "result": "female",
                        "result_status": "WIN",
                        "earnings": 975.0,
                        "handicap": null,
                        "is_combo": false,
                        "member_code": "4ucmqplxb029",
                        "is_unsettled": false,
                        "ticket_type": "live",
                        "malay_odds": 0.95,
                        "euro_odds": 1.95,
                        "member_odds": 0.95,
                        "member_odds_style": "hongkong",
                        "game_type_id": 27,
                        "request_source": "desktop-browser"
                    }
                ]
            }
            """;

        var uriBuilder = new UriBuilder(apiUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["from_settlement_datetime"] = startTime.ToUniversalTime().ToString("u").Replace(" ", "T");
        query["to_settlement_datetime"] = endTime.ToUniversalTime().ToString("u").Replace(" ", "T");
        query["page_size"] = pageSize.ToString();
        uriBuilder.Query = query.ToString();

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, uriBuilder.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .BetTransactionSettledAsync(
                apiUrl: apiUrl,
                privateToken: privateToken,
                startTime: startTime,
                endTime: endTime,
                pageSize: pageSize)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }

    [TestMethod]
    public async Task 查詢已結算含取消注單_單號CDNAE185486H4746KBX08EB()
    {
        var apiUrl = new Uri("https://spi-test.r4espt.com/api/v2/bet-transaction/");
        var privateToken = "0002fc0c96401e68419756f562689ed6e8e1ea398406f710d4d3f1c99b04f61b8fc0bea712b7d674f8be0dbb69a7a3aca0b02b423c73257eeb87521799ec7479218285597e314f5b18189eb8ebfc2bbf4dea799ad3b2fc74d99abb709ca86bc03d4daa5b17b078d4601152920903d18546c7335cb1008d2158f999ba0810a3643fcb";
        var startTime = DateTime.Parse("2023-01-04T05:43:37Z");
        var endTime = DateTime.Parse("2023-01-06T07:43:37Z");
        var pageSize = 100;

        var response = """
            {"count":10,"next":null,"previous":null,"results":[{"id":"CDNAE185486H5806UIIFF78","bet_selection":"home","odds":0.29,"currency":"RMB","amount":50.0,"game_type_name":"CS:GO","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2023 Gunz Blazing Tournament","event_id":13333598,"event_name":"0to100 vs 100pinggods","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T07:58:06.616284Z","settlement_datetime":"2023-01-06T02:33:46.463304Z","modified_datetime":"2023-01-06T02:33:46.756823Z","settlement_status":"settled","result":"5:4","result_status":"WIN","earnings":64.5,"handicap":null,"is_combo":false,"member_code":"4uglyyqu4nae","is_unsettled":true,"ticket_type":"db","malay_odds":0.29,"euro_odds":1.29,"member_odds":0.29,"member_odds_style":"hongkong","game_type_id":1,"request_source":"mobile-browser"},{"id":"CDNAE185486H4752NVW29EE","bet_selection":"home","odds":0.5,"currency":"RMB","amount":100.0,"game_type_name":"Dota 2","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2022 BTS Pro Series Season 12: Americas","event_id":13334326,"event_name":"2EZ Gaming vs 322 As An Opportunity","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T07:47:52.264424Z","settlement_datetime":"2023-01-06T02:18:51.186856Z","modified_datetime":"2023-01-06T02:18:51.366708Z","settlement_status":"settled","result":"3:4","result_status":"LOSS","earnings":-100.0,"handicap":null,"is_combo":false,"member_code":"4uglyyqu4nae","is_unsettled":false,"ticket_type":"db","malay_odds":0.5,"euro_odds":1.5,"member_odds":0.5,"member_odds_style":"hongkong","game_type_id":2,"request_source":"desktop-browser"},{"id":"CDNAE185486H4746KBX08EB","bet_selection":"home","odds":0.71,"currency":"RMB","amount":100.0,"game_type_name":"Overwatch","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2022 Overwatch Contenders Summer Series: ANZ B-Sides","event_id":13336302,"event_name":"ADSL2+ vs ATL Academy","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T07:47:46.003831Z","settlement_datetime":"2023-01-06T02:19:28.324106Z","modified_datetime":"2023-01-06T02:19:28.332143Z","settlement_status":"cancelled","result":null,"result_status":"CANCELLED","earnings":null,"handicap":null,"is_combo":false,"member_code":"4uglyyqu4nae","is_unsettled":false,"ticket_type":"db","malay_odds":0.71,"euro_odds":1.71,"member_odds":0.71,"member_odds_style":"hongkong","game_type_id":5,"request_source":"desktop-browser"},{"id":"CDNAE185486H4741WQAB9AC","bet_selection":"home","odds":0.71,"currency":"RMB","amount":100.0,"game_type_name":"Starcraft II","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2022 DH SC2 Masters Atlanta: China","event_id":13335648,"event_name":"Afreeca Freecs vs BLord","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T07:47:41.182015Z","settlement_datetime":"2023-01-06T02:22:19.596825Z","modified_datetime":"2023-01-06T02:22:19.876778Z","settlement_status":"settled","result":"3:3","result_status":"DRAW","earnings":100.0,"handicap":null,"is_combo":false,"member_code":"4uglyyqu4nae","is_unsettled":false,"ticket_type":"db","malay_odds":0.71,"euro_odds":1.71,"member_odds":0.71,"member_odds_style":"hongkong","game_type_id":4,"request_source":"desktop-browser"},{"id":"CDNAE185486H4736MZWADA9","bet_selection":"home","odds":0.5,"currency":"RMB","amount":100.0,"game_type_name":"Dota 2","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2022 BTS Pro Series Season 12: Americas","event_id":13334292,"event_name":"212 vs 2EZ Gaming","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T07:47:36.911627Z","settlement_datetime":"2023-01-06T02:22:46.496508Z","modified_datetime":"2023-01-06T02:22:46.723411Z","settlement_status":"settled","result":"5:3","result_status":"WIN","earnings":150.0,"handicap":null,"is_combo":false,"member_code":"4uglyyqu4nae","is_unsettled":false,"ticket_type":"db","malay_odds":0.5,"euro_odds":1.5,"member_odds":0.5,"member_odds_style":"hongkong","game_type_id":2,"request_source":"desktop-browser"},{"id":"CDNAE185486H4730KGD8F82","bet_selection":"home","odds":0.51,"currency":"RMB","amount":100.0,"game_type_name":"League of Legends","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2022 World Championship","event_id":13334969,"event_name":"17 Academy vs 1907 Fenerbahçe Academy","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T07:47:30.167120Z","settlement_datetime":"2023-01-06T02:23:52.043329Z","modified_datetime":"2023-01-06T02:23:52.314359Z","settlement_status":"settled","result":"4:3","result_status":"WIN","earnings":151.0,"handicap":null,"is_combo":false,"member_code":"4uglyyqu4nae","is_unsettled":false,"ticket_type":"db","malay_odds":0.51,"euro_odds":1.51,"member_odds":0.51,"member_odds_style":"hongkong","game_type_id":3,"request_source":"desktop-browser"},{"id":"CDNAE185486H4718NWIC0F3","bet_selection":"home","odds":0.29,"currency":"RMB","amount":100.0,"game_type_name":"CS:GO","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2023 Gunz Blazing Tournament","event_id":13333598,"event_name":"0to100 vs 100pinggods","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T07:47:18.013930Z","settlement_datetime":"2023-01-06T02:33:40.579785Z","modified_datetime":"2023-01-06T02:33:40.587189Z","settlement_status":"cancelled","result":"5:4","result_status":"CANCELLED","earnings":null,"handicap":null,"is_combo":false,"member_code":"4uglyyqu4nae","is_unsettled":true,"ticket_type":"db","malay_odds":0.29,"euro_odds":1.29,"member_odds":0.29,"member_odds_style":"hongkong","game_type_id":1,"request_source":"desktop-browser"},{"id":"CDNAE185273B4154DEMF2C1","odds":3.74,"currency":"RMB","amount":100.0,"game_market_name":null,"market_option":null,"map_num":null,"bet_type_name":null,"event_id":13334341,"event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T01:41:54.331992Z","settlement_datetime":"2023-01-06T03:04:14.751012Z","modified_datetime":"2023-01-06T03:04:15.015962Z","settlement_status":"settled","result_status":"LOSS","earnings":-100.0,"is_combo":true,"member_code":"4udxu8ivnr24","is_unsettled":false,"malay_odds":null,"euro_odds":3.74,"member_odds":3.74,"member_odds_style":"euro","request_source":"desktop-browser","tickets":[{"id":"CDNAE185273B4154SCE42F3","bet_selection":"home","odds":0.5,"game_type_name":"Dota 2","game_type_id":2,"game_market_name":"WINNER","bet_type_name":"WIN","competition_name":"2022 BTS Pro Series Season 12: Americas","event_id":13334341,"event_name":"3 Aegises 2 Anchors vs 425","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T01:41:54.340166Z","settlement_datetime":"2023-01-06T03:04:14.751012Z","settlement_status":"settled","result_status":"LOSS","handicap":null,"is_unsettled":false,"ticket_type":"db","malay_odds":0.5,"euro_odds":1.5,"member_odds":1.5,"member_odds_style":"euro","request_source":"desktop-browser","result":"3:5","market_option":"match","map_num":null},{"id":"CDNAE185273B4154FFJ903F","bet_selection":"home","odds":0.29,"game_type_name":"CS:GO","game_type_id":1,"game_market_name":"WINNER","bet_type_name":"WIN","competition_name":"2023 Gunz Blazing Tournament","event_id":13333579,"event_name":"00Prospects vs 100 Thieves","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T01:41:54.362352Z","settlement_datetime":"2023-01-06T03:03:13.847222Z","settlement_status":"settled","result_status":"WIN","handicap":null,"is_unsettled":false,"ticket_type":"db","malay_odds":0.29,"euro_odds":1.29,"member_odds":1.29,"member_odds_style":"euro","request_source":"desktop-browser","result":"5:3","market_option":"match","map_num":null},{"id":"CDNAE185273B4154AWWAFE5","bet_selection":"home","odds":0.5,"game_type_name":"Dota 2","game_type_id":2,"game_market_name":"WINNER","bet_type_name":"WIN","competition_name":"2022 BTS Pro Series Season 12: Americas","event_id":13334326,"event_name":"2EZ Gaming vs 322 As An Opportunity","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T01:41:54.351187Z","settlement_datetime":"2023-01-06T02:18:51.186856Z","settlement_status":"settled","result_status":"LOSS","handicap":null,"is_unsettled":false,"ticket_type":"db","malay_odds":0.5,"euro_odds":1.5,"member_odds":1.5,"member_odds_style":"euro","request_source":"desktop-browser","result":"3:4","market_option":"match","map_num":null},{"id":"CDNAE185273B4154ISKCBC1","bet_selection":"home","odds":0.29,"game_type_name":"CS:GO","game_type_id":1,"game_market_name":"WINNER","bet_type_name":"WIN","competition_name":"2023 Gunz Blazing Tournament","event_id":13333567,"event_name":"00Nation vs 0to100","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T01:41:54.374193Z","settlement_datetime":"2023-01-04T02:46:15.825930Z","settlement_status":"settled","result_status":"WIN","handicap":null,"is_unsettled":false,"ticket_type":"db","malay_odds":0.29,"euro_odds":1.29,"member_odds":1.29,"member_odds_style":"euro","request_source":"desktop-browser","result":"2:1","market_option":"match","map_num":null}]},{"id":"CDNAE185273B3945HQYED2A","bet_selection":"home","odds":0.29,"currency":"RMB","amount":200.0,"game_type_name":"CS:GO","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2023 Gunz Blazing Tournament","event_id":13333614,"event_name":"100 Thieves vs 1337HUANIA","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T01:39:45.535230Z","settlement_datetime":"2023-01-06T02:27:26.254910Z","modified_datetime":"2023-01-06T02:27:26.433700Z","settlement_status":"settled","result":"2:4","result_status":"LOSS","earnings":-200.0,"handicap":null,"is_combo":false,"member_code":"4udxu8ivnr24","is_unsettled":false,"ticket_type":"db","malay_odds":0.29,"euro_odds":1.29,"member_odds":0.29,"member_odds_style":"hongkong","game_type_id":1,"request_source":"desktop-browser"},{"id":"CDNAE185273B3938BHG7419","bet_selection":"home","odds":0.29,"currency":"RMB","amount":100.0,"game_type_name":"CS:GO","game_market_name":"WINNER","market_option":"match","map_num":null,"bet_type_name":"WIN","competition_name":"2023 Gunz Blazing Tournament","event_id":13333598,"event_name":"0to100 vs 100pinggods","event_datetime":"2023-01-04T15:59:59Z","date_created":"2023-01-04T01:39:38.454786Z","settlement_datetime":"2023-01-06T02:33:46.463304Z","modified_datetime":"2023-01-06T02:33:46.740022Z","settlement_status":"settled","result":"5:4","result_status":"WIN","earnings":129.0,"handicap":null,"is_combo":false,"member_code":"4udxu8ivnr24","is_unsettled":true,"ticket_type":"db","malay_odds":0.29,"euro_odds":1.29,"member_odds":0.29,"member_odds_style":"hongkong","game_type_id":1,"request_source":"desktop-browser"}]}
            """;

        var uriBuilder = new UriBuilder(apiUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["from_settlement_datetime"] = startTime.ToUniversalTime().ToString("u").Replace(" ", "T");
        query["to_settlement_datetime"] = endTime.ToUniversalTime().ToString("u").Replace(" ", "T");
        query["page_size"] = pageSize.ToString();
        uriBuilder.Query = query.ToString();

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, uriBuilder.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .BetTransactionSettledAsync(
                apiUrl: apiUrl,
                privateToken: privateToken,
                startTime: startTime,
                endTime: endTime,
                pageSize: pageSize)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}

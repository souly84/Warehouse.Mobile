using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Droid.Services;
using Warehouse.Mobile.IntegrationTests.AndroidInstrumentations;
using Warehouse.Mobile.ViewModels;
using Warehouse.Scanner.SDK;
using WebRequest.Elegant.Fakes;
using Xunit;

namespace Warehouse.Mobile.IntegrationTests
{
    public class MainActivityTests
    {
        private TestInstrumentation instrument = TestInstrumentation.CurrentInstrumentation;

        [Fact]
        public async Task ScannerOpened()
        {
            if (instrument != null)
            {
                using (var activity = new InstrumentationActivity<Droid.MainActivity>(instrument))
                {
                    var mainActivity = await activity.ActivityAsync();
                    Assert.NotNull(
                        mainActivity.App.Scanner
                    );

                    await mainActivity.App.Scanner
                        .WaitIfTransitionAsync(ScannerState.Closed)
                        .WithTimeout(30000);
                    Assert.Equal(
                        ScannerState.Opened,
                        mainActivity.App.Scanner.State
                    );
                }
            }
        }

        [Fact]
        public async Task NavigationCreated()
        {
            if (instrument != null)
            {
                using (var activity = new InstrumentationActivity<Droid.MainActivity>(instrument))
                {
                    Assert.NotNull(
                       (await activity.ActivityAsync()).Navigation
                    );
                }
            }
        }

        [Fact]
        public async Task MenuSelectionAsDefaultPage()
        {
            if (instrument != null)
            {
                using (var activity = new InstrumentationActivity<Droid.MainActivity>(instrument))
                {
                    var mainActivity = await activity.ActivityAsync();
                    Assert.IsType<MenuSelectionViewModel>(
                        mainActivity.App.CurrentViewModel<object>()
                    );
                }
            }
        }



        [Fact]
        public async Task RestoreReceptionState()
        {
            var reception = new EbSoftReception(
                new WebRequest.Elegant.WebRequest("http://example.com", new FkHttpMessageHandler(_data)), 9);
            var keyValueStorage = new SimpleKeyValueStorage("Warehouse_Mobile_Droid");
            keyValueStorage.Set<JObject>("Repcetion_9", JObject.Parse(@"{
                  ""5449000131805"": 1,
                  ""5410013108009"": 1,
                  ""4005176891021"": 1
                }"));

            var result = await new StatefulReception(reception
                .WithExtraConfirmed()
                .WithoutInitiallyConfirmed(),
                keyValueStorage)
                .NotConfirmedOnly()
                .ToViewModelListAsync();
        }

        private string _data = @"[
            {""id"":""40"",""oa_dossier"":""OA831375"",""article"":""GROHE 3328130E"",""qt"":""1"",""ean"":[""4005176635410""],""qtin"":""1"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""41"",""oa_dossier"":""OA831375"",""article"":""GROHE 3328130E"",""qt"":""1"",""ean"":[""4005176635410""],""qtin"":""1"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""42"",""oa_dossier"":""OA831375"",""article"":""GROHE 3328120E"",""qt"":""1"",""ean"":[""4005176868498""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""43"",""oa_dossier"":""OA859840"",""article"":""GROHE 31566SD0"",""qt"":""1"",""ean"":[""4005176473234""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""44"",""oa_dossier"":""OA859840"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""45"",""oa_dossier"":""OA859840"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""46"",""oa_dossier"":""OA859840"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""47"",""oa_dossier"":""OA861069"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""0""}]";

    }
}

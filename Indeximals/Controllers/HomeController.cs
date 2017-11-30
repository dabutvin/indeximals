using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Indeximals.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Indeximals.Controllers
{
    public class HomeController : Controller
    {
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudBlobContainer _animalsContainer;
        private readonly ISearchIndexClient _searchIndexClient;

        public HomeController(CloudBlobClient cloudBlobClient, ISearchIndexClient searchIndexClient)
        {
            _cloudBlobClient = cloudBlobClient;
            _animalsContainer = _cloudBlobClient.GetContainerReference("animals");
            _searchIndexClient = searchIndexClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(Animal model)
        {
            if(model?.Id != null)
            {
                await _animalsContainer.CreateIfNotExistsAsync();

                var blob = _animalsContainer.GetBlockBlobReference($"{model.Id}.json");
                blob.Properties.ContentType = "application/json";
                await blob.UploadTextAsync(JsonConvert.SerializeObject(model));
            }

            return View(model);
        }

        public async Task<IActionResult> List()
        {
            await _animalsContainer.CreateIfNotExistsAsync();
            var animalBlobs = await _animalsContainer.ListBlobsSegmentedAsync(null);

            var animalJsons = await Task.WhenAll(animalBlobs.Results
                .OfType<CloudBlockBlob>()
                .Select(x => x.DownloadTextAsync()));

            return View(animalJsons
                .Select(x => JsonConvert.DeserializeObject<Animal>(x))
                .ToArray());
        }

        public async Task<IActionResult> Query(QueryViewModel model)
        {
            var response = await _searchIndexClient.Documents.SearchAsync<Animal>(model.Term);

            model.Animals = response.Results.Select(x => x.Document).ToArray();

            return View(model);
        }

        public async Task<IActionResult> Suggest(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(null);
            }

            var suggestions = await _searchIndexClient.Documents.SuggestAsync<Animal>(term, "suggester");

            return Json(suggestions.Results.Select(x => KeyValuePair.Create(x.Text, x.Document)));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

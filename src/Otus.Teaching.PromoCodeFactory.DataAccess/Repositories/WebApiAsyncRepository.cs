using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;
using Otus.Teaching.PromoCodeFactory.Core;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Repositories
{
    public class WebApiAsyncRepository<T> : IAsyncRepositoryT<T> where T : BaseEntity
    {
        //GenericRepository на webApi
        
        private static readonly object _locker = new object();

        private HttpClient httpClient;
        
        string _getAllPrefix;
        string _addItemPrefix;
        string _updateItemPrefix;
        string _deleteItemPrefix;
        bool _getPostOnlyMode; //если true, то репозиторий делает операции put и delete через post, т.к. у хостера put и delete выдают 405

        public string Guid {get; private set;}
        public string DbContextGuid { get { return ""; } }

        public WebApiAsyncRepository(string baseAddress)
        {
            httpClient = new System.Net.Http.HttpClient(new HttpClientHandler());
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task<int> Count
        {
            get
            {
                int rez;
                try
                {
                    Task<HttpResponseMessage> response;
                    string json;
                    response = httpClient.GetAsync($"Count/");

                    //тут возвращается not found 404
                    switch (response.Result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            json = response.Result.Content.ReadAsStringAsync().Result;
                            rez = JsonConvert.DeserializeObject<int>(json);
                            break;
                        case System.Net.HttpStatusCode.NotFound:
                        default:
                            rez = -1;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    rez = -1;
                }
                return Task.FromResult(rez);
            }
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> items;

            try
            {
                Task<HttpResponseMessage> response;
                string json;
                response = httpClient.GetAsync("getall/");
                json = response.Result.Content.ReadAsStringAsync().Result;
                items = JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebApiRepository error: {ex.Message}");
                items = (IEnumerable<T>)new List<T>();
            }
            return Task.FromResult(items);
        }

        public Task<T> GetByIdOrNullAsync(Guid id)
        {
            T item;
            
            try
            {
                Task<HttpResponseMessage> response;

                string json;

                response = httpClient.GetAsync($"GetByIdOrNull/{id}");

                //тут возвращается not found 404
                switch (response.Result.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        json = response.Result.Content.ReadAsStringAsync().Result;
                        item = JsonConvert.DeserializeObject<T>(json);
                        break;

                    case System.Net.HttpStatusCode.NotFound:
                        item=null;
                        break;

                    default:
                        item = null;
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebApiRepository error: {ex.Message}");
                item = null;
            }
            return Task.FromResult(item);
        }

        public Task<bool> Exists(Guid id)
        {
            return Task.FromResult(GetByIdOrNullAsync(id).Result != null);
        }

        public Task<CommonOperationResult> AddAsync(T t)
        {
            CommonOperationResult rez;
            try
            {
                Task<HttpResponseMessage> response;
                string json;
                StringContent jsonContent;

                json = JsonConvert.SerializeObject(t, Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                response = httpClient.PostAsync($"", jsonContent);

                switch (response.Result.StatusCode)
                {
                    default:
                    case System.Net.HttpStatusCode.OK:
                        rez = CommonOperationResult.sayOk(response.Result.Content.ReadAsStringAsync().Result);
                        break;

                    case System.Net.HttpStatusCode.Conflict:
                        rez = CommonOperationResult.sayFail(response.Result.Content.ReadAsStringAsync().Result);
                        break;
                }
            }
            catch (Exception ex)
            {
                rez = CommonOperationResult.sayFail($"WebApiRepository error: {ex.Message}");
            }
            return Task.FromResult(rez);
        }


        public Task<CommonOperationResult> UpdateAsync(T t)
        {

            CommonOperationResult rez;
            try
            {
                Task<HttpResponseMessage> response;
                string json;
                StringContent jsonContent;

                json = JsonConvert.SerializeObject(t, Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                response = httpClient.PutAsync($"", jsonContent);

                switch (response.Result.StatusCode)
                {
                    default:
                    case System.Net.HttpStatusCode.OK:
                        rez = CommonOperationResult.sayOk(response.Result.Content.ReadAsStringAsync().Result);
                        break;

                    case System.Net.HttpStatusCode.Conflict:
                        rez = CommonOperationResult.sayFail(response.Result.Content.ReadAsStringAsync().Result);
                        break;
                }
                // Console.WriteLine($"Server reply: code={response.Result.StatusCode} rezult={}");
            }
            catch (Exception ex)
            {
                rez = CommonOperationResult.sayFail($"WebApiRepository error: {ex.Message}");
            }
            return Task.FromResult(rez);
        }

        public Task<CommonOperationResult> DeleteAsync(Guid id)
        {

            CommonOperationResult rez;
            try
                {
                    Task<HttpResponseMessage> response;
                    response = httpClient.DeleteAsync($"{id}");

                switch (response.Result.StatusCode)
                {
                    default:
                    case System.Net.HttpStatusCode.OK:
                        rez = CommonOperationResult.sayOk(response.Result.Content.ReadAsStringAsync().Result);
                        break;

                    case System.Net.HttpStatusCode.Conflict:
                        rez = CommonOperationResult.sayFail(response.Result.Content.ReadAsStringAsync().Result);
                        break;
                }
            }
            catch (Exception ex)
            {
                rez = CommonOperationResult.sayFail($"WebApiRepository error: {ex.Message}");
            }
            return Task.FromResult(rez);

        }

        Task<List<T>> IAsyncRepositoryT<T>.GetItemsListAsync()
        {
            List<T> rez = new List<T>();

            IEnumerable<T> list = GetAllAsync().Result;

            foreach (T t in list)
            {
                rez.Add(t);
            }
            return Task.FromResult(rez);
        }

        Task<CommonOperationResult> IAsyncRepositoryT<T>.InitAsync(bool deleteDb)
        {
            CommonOperationResult rez = CommonOperationResult.sayOk();
            return Task.FromResult(rez);
        }

        public Task<T> GetRandomObject()
        {
            Random random = new Random();
            List<T> list = GetAllAsync().Result.ToList();
            int count = list.Count;
            int rndNum = random.Next(0, count);
            T t = list[rndNum];
            return Task.FromResult(t);
        }
    }
}
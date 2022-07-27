namespace Module5HW1
{
    using System.Net;
    using Newtonsoft.Json;
    using Module5HW1.Models;
    using System.Text;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ShowUserList(2);
            await ShowUserById(2);
            await ShowResourceList(2);
            await ShowResourceById(2);
            await CreateUser("Alex", "Programmer");
            await UserPutRequest(1, "Alex", "Programmer");
            await UserPatchRequest(2, "Vladimir", "Designer");
            await DeleteUser(2);
            await RegisterUser("janet.weaver@reqres.in", "somepass");
            await RegisterUser("janet.weaver@reqres.in", null);
            await LoginUser("janet.weaver@reqres.in", "somepass");
            await LoginUser("janet.weaver@reqres.in", null);
            await ShowUserList(2, 10);
        }

        public static async Task ShowUserList(int? page, int? delay = 0)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var result = await httpClient.GetAsync($"api/users?page={page}&delay={delay}");
                string? pageInfoToDeserialize = await result.Content.ReadAsStringAsync();
                PageInfo<User>? pageInfo;

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    pageInfo = JsonConvert.DeserializeObject<PageInfo<User>>(pageInfoToDeserialize);

                    if (pageInfo != null)
                    {
                        Console.WriteLine($"Page: {pageInfo.Page}\nPer page: {pageInfo.PerPage}\nTotal: {pageInfo.Total}\nTotal pages: {pageInfo.TotalPages}\nUsers:");
                        
                        if (pageInfo.Data != null && pageInfo.Data.Count != 0)
                        {
                            foreach(var user in pageInfo.Data)
                            {
                                Console.WriteLine($"Id: {user.Id}\nFirst name: {user.FirstName}\nLast name: {user.LastName}\nEmail: {user.Email}\nAvatar: {user.Avatar}\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data is null or empty\n");
                        }
                    }
                        
                }
            }
        }

        public static async Task ShowUserById(int? id)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var result = await httpClient.GetAsync($"api/users/{id}");
                string? userInfoToDeserialize = await result.Content.ReadAsStringAsync();
                ModelData<User>? userData;

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    userData = JsonConvert.DeserializeObject<ModelData<User>>(userInfoToDeserialize);

                    if (userData != null && userData.Data != null)
                    {

                        Console.WriteLine($"User: ");
                        Console.WriteLine($"Id: {userData.Data.Id}\nFirst name: {userData.Data.FirstName}\nLast name: {userData.Data.LastName}\nEmail: {userData.Data.Email}\nAvatar: {userData.Data.Avatar}\n");
                    }
                    else
                    {
                        Console.WriteLine("Data is null");
                    }
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Not found");
                }
            }
        }

        public static async Task ShowResourceList(int? page)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var result = await httpClient.GetAsync($"api/unknown?page={page}");
                string? pageInfoToDeserialize = await result.Content.ReadAsStringAsync();
                PageInfo<Resource>? pageInfo;

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    pageInfo = JsonConvert.DeserializeObject<PageInfo<Resource>>(pageInfoToDeserialize);

                    if (pageInfo != null)
                    {
                        Console.WriteLine($"Page: {pageInfo.Page}\nPer page: {pageInfo.PerPage}\nTotal: {pageInfo.Total}\nTotal pages: {pageInfo.TotalPages}\nResources:");

                        if (pageInfo.Data != null && pageInfo.Data.Count != 0)
                        {
                            foreach (var resource in pageInfo.Data)
                            {
                                Console.WriteLine($"Id: {resource.Id}\nName: {resource.Name}\nYear: {resource.Year}\nColor: {resource.Color}\nPantone value: {resource.PantoneValue}\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data is null or empty\n");
                        }
                    }

                }
            }
        }

        public static async Task ShowResourceById(int? id)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var result = await httpClient.GetAsync($"api/unknown/{id}");
                string? resourceDataToDeserialize = await result.Content.ReadAsStringAsync();
                ModelData<Resource>? resourceData;

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    resourceData = JsonConvert.DeserializeObject<ModelData<Resource>>(resourceDataToDeserialize);

                    if (resourceData != null && resourceData.Data != null)
                    {

                        Console.WriteLine($"Resource: ");
                        Console.WriteLine($"Id: {resourceData.Data.Id}\nName: {resourceData.Data.Name}\nYear: {resourceData.Data.Year}\nColor: {resourceData.Data.Color}\nPantone value: {resourceData.Data.PantoneValue}\n");
                    }
                    else
                    {
                        Console.WriteLine("Data is null");
                    }
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Not found");
                }
            }
        }

        public static async Task CreateUser(string name, string job)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var user = new { Name = name, Job = job };
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.Unicode, "application/json");
                var result = await httpClient.PostAsync($"api/users", content);

                if (result.StatusCode == HttpStatusCode.Created)
                {
                    Console.WriteLine("Created!");

                    var response = await result.Content.ReadAsStringAsync();
                    var createdUser = JsonConvert.DeserializeObject<CreatedUser>(response);

                    if (createdUser != null)
                    {
                        Console.WriteLine($"Id: {createdUser.Id}\nName: {createdUser.Name}\nJob: {createdUser.Job}\nCreated at: {createdUser.CreatedAt}\n");
                    } 
                }
            }
        }

        public static async Task UserPutRequest(int id, string? name, string? job)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var user = new { Name = name, Job = job };
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.Unicode, "application/json");
                var result = await httpClient.PutAsync($"api/users/{id}", content);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Updated!");

                    var response = await result.Content.ReadAsStringAsync();
                    var updatedUser = JsonConvert.DeserializeObject<UpdatedUser>(response);

                    if (updatedUser != null)
                    {
                        Console.WriteLine($"Name: {updatedUser.Name}\nJob: {updatedUser.Job}\nUpdated at: {updatedUser.UpdatedAt}\n");
                    }
                }
                else
                {
                    Console.WriteLine(result.StatusCode);
                }
            }
        }

        public static async Task UserPatchRequest(int id, string? name, string? job)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var user = new { Name = name, Job = job };
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.Unicode, "application/json");
                var result = await httpClient.PatchAsync($"api/users/{id}", content);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Updated!");

                    var response = await result.Content.ReadAsStringAsync();
                    var updatedUser = JsonConvert.DeserializeObject<UpdatedUser>(response);

                    if (updatedUser != null)
                    {
                        Console.WriteLine($"Name: {updatedUser.Name}\nJob: {updatedUser.Job}\nUpdated at: {updatedUser.UpdatedAt}\n");
                    }
                }
                else
                {
                    Console.WriteLine(result.StatusCode);
                }
            }
        }

        public static async Task DeleteUser(int id)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var result = await httpClient.DeleteAsync($"api/users/{id}");

                if (result.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.WriteLine("Deleted!\n");
                }
            }
        }

        public static async Task RegisterUser(string? email, string? pass)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var credentials = new { email = email, password = pass };
                var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.Unicode, "application/json");
                var result = await httpClient.PostAsync($"api/register", content);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Registered!");

                    var response = await result.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<AuthResponse>(response);

                    if (token != null)
                        Console.WriteLine($"Token: {token.Token}\nUser id: {token.Id}\n");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    var response = await result.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorMessage>(response);

                    if (error != null)
                        Console.WriteLine($"Error: {error.Error}\n");
                }
            }
        }

        public static async Task LoginUser(string? email, string? pass)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/") })
            {
                var credentials = new { email = email, password = pass };
                var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.Unicode, "application/json");
                var result = await httpClient.PostAsync($"api/login", content);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Logged in!");

                    var response = await result.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<AuthResponse>(response);

                    if (token != null)
                        Console.WriteLine($"Token: {token.Token}\n");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    var response = await result.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorMessage>(response);

                    if (error != null)
                        Console.WriteLine($"Error: {error.Error}\n");
                }
            }
        }
    }
}
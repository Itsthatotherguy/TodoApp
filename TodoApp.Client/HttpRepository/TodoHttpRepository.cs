using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApp.Models.Todo.Requests;
using TodoApp.Models.Todo.Responses;
using TodoApp.Utilities;

namespace TodoApp.Client.HttpRepository
{
    public class TodoHttpRepository : ITodoHttpRepository
    {
        private readonly HttpClient _httpClient;

        public TodoHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<List<ListTodosResponseModel>>> GetTodos()
        {
            var response = await _httpClient.GetAsync("api/Todo");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var todos = JsonSerializer.Deserialize<ListTodosResponseModel[]>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Result<List<ListTodosResponseModel>>.Success(todos.ToList());
            }
            else
            {
                var errors = JsonSerializer.Deserialize<string[]>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Result<List<ListTodosResponseModel>>.Failure(errors.ToList());
            }
            
        }

        public async Task<Result> CreateTodo(CreateTodoRequestModel model)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json");

            var postResult = await _httpClient.PostAsync("api/Todo", content);

            if (!postResult.IsSuccessStatusCode)
            {
                var errors = JsonSerializer.Deserialize<List<string>>(
                    await postResult.Content.ReadAsStringAsync(), 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Result.Failure(errors);
            }

            return Result.Success();
        }

        public async Task<Result<ReadTodoResponseModel>> GetOneTodo(Guid id)
        {
            var getResult = await _httpClient.GetAsync($"api/Todo/{id}");

            var getContent = await getResult.Content.ReadAsStringAsync();

            if (getResult.IsSuccessStatusCode)
            {
                var todo = JsonSerializer.Deserialize<ReadTodoResponseModel>(
                    getContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Result<ReadTodoResponseModel>.Success(todo);
            }
            else
            {
                var errors = JsonSerializer.Deserialize<string[]>(
                    getContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Result<ReadTodoResponseModel>.Failure(errors.ToList());
            }
        }

        public async Task<Result> UpdateTodo(UpdateTodoRequestModel model)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json");

            var updateResult = await _httpClient.PutAsync($"api/Todo/{model.Id}", content);

            if (!updateResult.IsSuccessStatusCode)
            {
                var errors = JsonSerializer.Deserialize<string[]>(
                    await updateResult.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Result.Failure(errors.ToList());
            }

            return Result.Success();
        }

        public async Task<Result> DeleteTodo(Guid id)
        {
            var deleteResult = await _httpClient.DeleteAsync($"api/Todo/{id}");

            if (!deleteResult.IsSuccessStatusCode)
            {
                var errors = JsonSerializer.Deserialize<List<string>>(
                    await deleteResult.Content.ReadAsStringAsync());

                return Result.Failure(errors);
            }

            return Result.Success();
        }
    }
}

using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models.Todo;
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

        public async Task<Result<List<GetAllTodosDto>>> GetTodos()
        {
            var response = await _httpClient.GetAsync("api/Todo");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var todos = JsonConvert.DeserializeObject<GetAllTodosDto[]>(content);

                return Result<List<GetAllTodosDto>>.Success(todos.ToList());
            }
            else
            {
                var errors = JsonConvert.DeserializeObject<string[]>(content);

                return Result<List<GetAllTodosDto>>.Failure(errors.ToList());
            }
            
        }

        public async Task<Result> CreateTodo(CreateTodoDto model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            var postResult = await _httpClient.PostAsync("api/Todo", content);

            if (!postResult.IsSuccessStatusCode)
            {
                var errors = JsonConvert.DeserializeObject<List<string>>(
                    await postResult.Content.ReadAsStringAsync());

                return Result.Failure(errors);
            }

            return Result.Success();
        }

        public async Task<Result<GetOneTodoDto>> GetOneTodo(Guid id)
        {
            var getResult = await _httpClient.GetAsync($"api/Todo/{id}");

            var getContent = await getResult.Content.ReadAsStringAsync();

            if (getResult.IsSuccessStatusCode)
            {
                var todo = JsonConvert.DeserializeObject<GetOneTodoDto>(getContent);

                return Result<GetOneTodoDto>.Success(todo);
            }
            else
            {
                var errors = JsonConvert.DeserializeObject<string[]>(getContent);

                return Result<GetOneTodoDto>.Failure(errors.ToList());
            }
        }

        public async Task<Result> UpdateTodo(UpdateTodoDto model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            var updateResult = await _httpClient.PutAsync($"api/Todo/{model.Id}", content);

            if (!updateResult.IsSuccessStatusCode)
            {
                var errors = JsonConvert.DeserializeObject<string[]>(
                    await updateResult.Content.ReadAsStringAsync());

                return Result.Failure(errors.ToList());
            }

            return Result.Success();
        }

        public async Task<Result> DeleteTodo(Guid id)
        {
            var deleteResult = await _httpClient.DeleteAsync($"api/Todo/{id}");

            if (!deleteResult.IsSuccessStatusCode)
            {
                var errors = JsonConvert.DeserializeObject<List<string>>(
                    await deleteResult.Content.ReadAsStringAsync());

                return Result.Failure(errors);
            }

            return Result.Success();
        }

        public async Task<Result> MarkTodoComplete(Guid id)
        {
            var patchDoc = new JsonPatchDocument<UpdateTodoDto>()
                .Replace(todo => todo.IsCompleted, true);

            var content = new StringContent(
                JsonConvert.SerializeObject(patchDoc),
                Encoding.UTF8,
                "application/json");

            var partialUpdateResult = await _httpClient.PatchAsync($"api/Todo/{id}", content);

            if (!partialUpdateResult.IsSuccessStatusCode)
            {
                var errors = JsonConvert.DeserializeObject<string[]>(
                    await partialUpdateResult.Content.ReadAsStringAsync());

                return Result.Failure(errors.ToList());
            }

            return Result.Success();
        }

        public async Task<Result> MarkTodoIncomplete(Guid id)
        {
            var patchDoc = new JsonPatchDocument<UpdateTodoDto>()
                .Replace(todo => todo.IsCompleted, false);

            var content = new StringContent(
                JsonConvert.SerializeObject(patchDoc),
                Encoding.UTF8,
                "application/json");

            var partialUpdateResult = await _httpClient.PatchAsync($"api/Todo/{id}", content);

            if (!partialUpdateResult.IsSuccessStatusCode)
            {
                var errors = JsonConvert.DeserializeObject<string[]>(
                    await partialUpdateResult.Content.ReadAsStringAsync());

                return Result.Failure(errors.ToList());
            }

            return Result.Success();
        }
    }
}

using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using TodoApp.IntegrationTests.Fixtures;
using TodoApp.IntegrationTests.TestData;
using TodoApp.Models.Todo;
using Xunit;

namespace TodoApp.IntegrationTests.Tests
{
    public class TodoControllerTests : BaseIntegrationTest
    {
        public TodoControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        #region POST api/Todo
        [Fact]
        [Trait("Endpoint", "POST api/Todo")]
        public async void Create_ValidInput_ReturnsCreated()
        {
            // Arrange
            var model = new CreateTodoDto
            {
                Title = "Wash clothes",
                Description = "I need clothes for the week",
                DueDate = DateTime.Today.AddDays(7)
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PostAsync("api/Todo", content);
            res.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseModel = JsonConvert.DeserializeObject<GetOneTodoDto>(
                await res.Content.ReadAsStringAsync());

            // Assert
            responseModel.Should().NotBeNull();
            responseModel.Id.Should().NotBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TodoControllerData.InvalidCreateData), MemberType = typeof(TodoControllerData))]
        [Trait("Endpoint", "POST api/Todo")]
        public async void Create_InvalidInput_ReturnsBadRequest(string title, DateTime dueDate)
        {
            // Arrange
            var model = new CreateTodoDto
            {
                Title = title,
                DueDate = dueDate
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PostAsync("api/Todo", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "POST api/Todo")]
        public async void Create_TitleThatAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var model = new CreateTodoDto
            {
                Title = "Walk the dogs", // this todo already exists
                DueDate = DateTime.Today
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PostAsync("api/Todo", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region GET api/Todo
        [Fact]
        [Trait("Endpoint", "GET api/Todo")]
        public async void GetAll_ReturnsOk()
        {
            // Act
            var res = await _client.GetAsync("api/Todo");
            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await res.Content.ReadAsStringAsync());

            // Assert
            responseModel.Should().HaveCount(2);
        }
        #endregion

        #region GET api/Todo/{id}
        [Fact]
        [Trait("Endpoint", "GET api/Todo/{id}")]
        public async void GetOne_ValidId_ReturnsOK()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            // Act
            var res = await _client.GetAsync($"api/Todo/{todoId}");
            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseModel = JsonConvert.DeserializeObject<GetOneTodoDto>(
                await res.Content.ReadAsStringAsync());

            // Assert
            responseModel.Should().NotBeNull();
            responseModel.Id.Should().Be(todoId);
        }

        [Fact]
        [Trait("Endpoint", "GET api/Todo/{id}")]
        public async void GetOne_InvalidId_ReturnsBadRequest()
        {
            var res = await _client.GetAsync($"api/Todo/{Guid.NewGuid()}");

            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "GET api/Todo/{id}")]
        public async void GetOne_EmptyId_ReturnsBadRequest()
        {
            var res = await _client.GetAsync($"api/Todo/{Guid.Empty}");

            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
        #endregion

        #region PUT api/Todo/{id}
        [Fact]
        [Trait("Endpoint", "PUT api/Todo/{id}")]
        public async void Update_ValidInput_ReturnsNoContent()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            var model = new UpdateTodoDto
            {
                Id = todoId,
                Title = "This is an updated title",
                Description = "This is an updated description",
                DueDate = DateTime.Now,
                IsCompleted = true
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PutAsync($"api/Todo/{todoId}", content);
            res.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getOneRes = await _client.GetAsync($"api/Todo/{todoId}");

            var getOneResponseModel = JsonConvert.DeserializeObject<GetOneTodoDto>(
                await getOneRes.Content.ReadAsStringAsync());

            // Assert
            getOneResponseModel.Title.Should().Be("This is an updated title");
            getOneResponseModel.Description.Should().Be("This is an updated description");
            getOneResponseModel.IsCompleted.Should().BeTrue();
        }

        [Fact]
        [Trait("Endpoint", "PUT api/Todo/{id}")]
        public async void Update_EmptyId_ReturnsBadRequest()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            var model = new UpdateTodoDto
            {
                Id = todoId
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PutAsync($"api/Todo/{Guid.Empty}", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "PUT api/Todo/{id}")]
        public async void Update_MismatchedIds_ReturnsBadRequest()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            var model = new UpdateTodoDto
            {
                Id = todoId
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PutAsync($"api/Todo/{Guid.NewGuid()}", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "PUT api/Todo/{id}")]
        public async void Update_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            var model = new UpdateTodoDto();

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PutAsync($"api/Todo/{todoId}", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "PUT api/Todo/{id}")]
        public async void Update_TitleAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var firstTodoId = getAllResponseModel[0].Id;
            var secondTodoId = getAllResponseModel[1].Id;

            var model = new UpdateTodoDto
            {
                Id = secondTodoId,
                Title = getAllResponseModel[0].Title
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PutAsync($"api/Todo/{secondTodoId}", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "PUT api/Todo/{id}")]
        public async void Update_TitleAlreadyExistsForSameTodo_ReturnsOk()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todo = getAllResponseModel[0];

            var model = new UpdateTodoDto
            {
                Id = todo.Id,
                Title = getAllResponseModel[0].Title,
                DueDate = todo.DueDate
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PutAsync($"api/Todo/{todo.Id}", content);
            res.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }
        #endregion

        #region PATCH api/Todo/{id}
        [Fact]
        [Trait("Endpoint", "PATCH api/Todo/{id}")]
        public async void Patch_ValidInput_ReturnsNoContent()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            var patchDoc = new[]
                {
                    new {
                    value = "true",
                    path = "/isCompleted",
                    op = "replace"
                }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(patchDoc),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PatchAsync($"api/Todo/{todoId}", content);
            res.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getOneRes = await _client.GetAsync($"api/Todo/{todoId}");

            var getOneResponseModel = JsonConvert.DeserializeObject<GetOneTodoDto>(
                await getOneRes.Content.ReadAsStringAsync());

            // Assert
            getOneResponseModel.IsCompleted.Should().BeTrue();
        }

        [Fact]
        [Trait("Endpoint", "PATCH api/Todo/{id}")]
        public async void Patch_EmptyId_ReturnsBadRequest()
        {
            // Arrange
            var todoId = Guid.Empty;

            var patchDoc = new[]
                {
                    new {
                    value = "true",
                    path = "/isCompleted",
                    op = "replace"
                }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(patchDoc),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PatchAsync($"api/Todo/{todoId}", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "PATCH api/Todo/{id}")]
        public async void Patch_InvalidPatchDoc_ReturnsBadRequest()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            var patchDoc = new
            {
                value = "true",
                path = "/1sCompl3t3d",
                op = "replace"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(patchDoc),
                Encoding.UTF8,
                "application/json");

            // Act
            var res = await _client.PatchAsync($"api/Todo/{todoId}", content);
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region DELETE api/Todo/{id}
        [Fact]
        [Trait("Endpoint", "DELETE api/Todo/{id}")]
        public async void Delete_ValidId_ReturnsNoContent()
        {
            // Arrange
            var getAllRes = await _client.GetAsync("api/Todo");

            var getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());

            var todoId = getAllResponseModel[1].Id;

            // Act
            var res = await _client.DeleteAsync($"api/Todo/{todoId}");
            res.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getAllRes = await _client.GetAsync("api/Todo");
            getAllResponseModel = JsonConvert.DeserializeObject<GetAllTodosDto[]>(
                await getAllRes.Content.ReadAsStringAsync());
            getAllResponseModel.Should().HaveCount(1);

            var getOneRes = await _client.GetAsync($"api/Todo/{todoId}");
            getOneRes.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "DELETE api/Todo/{id}")]
        public async void Delete_EmptyId_ReturnsBadRequest()
        {
            // Act
            var res = await _client.DeleteAsync($"api/Todo/{Guid.Empty}");
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Endpoint", "DELETE api/Todo/{id}")]
        public async void Delete_InvalidId_ReturnsBadRequest()
        {
            // Act
            var res = await _client.DeleteAsync($"api/Todo/{Guid.NewGuid()}");
            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion        
    }
}

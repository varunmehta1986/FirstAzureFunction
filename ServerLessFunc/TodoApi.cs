using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ServerLessFunc
{
	public static class TodoApi
	{
		static List<Todo> items = new List<Todo>();
		[FunctionName("CreateTodo")]
		public static async Task<IActionResult> CreateTodo(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("Create a new Todo list item");
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var toDoItem = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);
			var todo = new Todo() { TaskDescription = toDoItem.TaskDescription };

			items.Add(todo);
			return new OkObjectResult(todo);
		}
		[FunctionName("GetTodos")]
		public static IActionResult GetTodos(
		   [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req,
		   ILogger log)
		{
			log.LogInformation("Get ToDo Items List");
			return new OkObjectResult(items);
		}

		[FunctionName("GetTodosById")]
		public static IActionResult GetTodosById(
		   [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req,
		   ILogger log, string id)
		{
			log.LogInformation("Get ToDo Items List");
			var item = items.Where(item => item.Id == id).FirstOrDefault();
			if(item == null)
			{
				return new NotFoundResult();
			}
			return new OkObjectResult(item);;
		}

		[FunctionName("UpdateTodo")]
		public static async Task<IActionResult> UpdateTodo(
		   [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")] HttpRequest req,
		   ILogger log, string id)
		{
			log.LogInformation("Get ToDo Items List");
			var todo = items.Where(item => item.Id == id).FirstOrDefault();
			if (todo == null)
			{
				return new NotFoundResult();
			}

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);

			todo.IsCompleted = updated.IsCompleted;

			if (!string.IsNullOrEmpty(updated.TaskDescription))
			{
				todo.TaskDescription = updated.TaskDescription;
			}

			return new OkObjectResult(todo);
		}

		[FunctionName("DeleteTodo")]
		public static IActionResult DeleteTodo(
		   [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")] HttpRequest req,
		   ILogger log, string id)
		{
			log.LogInformation("Get ToDo Items List");
			var item = items.Where(item => item.Id == id).FirstOrDefault();
			if (item == null)
			{
				return new NotFoundResult();
			}
			items.Remove(item);
			return new OkResult(); ;
		}
	}
}
	
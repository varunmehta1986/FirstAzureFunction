using System;

namespace ServerLessFunc
{
	internal class Todo
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string TaskDescription { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public bool IsCompleted { get; set; }
	}

	internal class TodoCreateModel
	{
		public string TaskDescription { get; set; }
	}
	internal class TodoUpdateModel
	{
		public bool IsCompleted { get; set; }
		public string TaskDescription { get; set; }
	}
}
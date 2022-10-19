namespace Docs.Models
{

	public class Folder
	{
		public string Dir { get; set; }
		public List<Folder> Folders { get; set; } = new List<Folder>();
		public List<string> Files { get; set; } = new List<string>();
        public Folder(string dir)
        {
            Dir = dir;
        }
    }


	public class Contents
	{
		public string Name { get; set; }	
		public Folder Root { get; set; }


    }
}

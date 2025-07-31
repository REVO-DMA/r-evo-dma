namespace EnigmaRunner
{
    public class EnigmaProjectHelper
    {
        private readonly string _projectName;
        private readonly string[] _projectFileLines;
        
        public EnigmaProjectHelper(string projectFilePath)
        {
            _projectName = projectFilePath;
            _projectFileLines = File.ReadAllLines(projectFilePath);
        }

        public bool PatchTarget(string target, string newData)
        {
            for (int i = 0; i < _projectFileLines.Length; i++)
            {
                string line = _projectFileLines[i];

                if (line == null)
                    continue;

                if (line.Contains(target, StringComparison.OrdinalIgnoreCase))
                {
                    _projectFileLines[i] = line.Replace(target, newData);
                    return true;
                }
            }

            Console.WriteLine($"[!] [ENIGMA PROJECT HELPER] -> PatchTarget(): Unable to locate \"{target}\" in project file!");

            return false;
        }

        public string GetTempFile(string extension)
        {
            string path = Path.GetTempFileName();

            string newExt;
            if (extension.StartsWith('.'))
                newExt = extension;
            else
                newExt = $".{extension}";

            string newFilePath = path.Replace(Path.GetExtension(path), newExt);
            File.Move(path, newFilePath);

            return newFilePath;
        }

        public string Save()
        {
            string path = GetTempFile(Path.GetExtension(_projectName));

            File.WriteAllLines(path, _projectFileLines);

            return path;
        }
    }
}

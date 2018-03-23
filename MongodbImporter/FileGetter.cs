using System.Collections.Generic;
using System.IO;
namespace MongodbImporter
{
    public class FileGetter
    {
        public string[] FileCollection;

        /// <summary>
        /// 根据路径获取路径下的备份文件
        /// </summary>
        /// <param name="filePath"></param>
        public FileGetter(string filePath)
        {
           FileCollection =  Directory.GetFiles(filePath,"*.bson",SearchOption.AllDirectories);
        }
        /// <summary>
        /// 获取Mongodb备份文件
        /// </summary>
        /// <returns>备份文件集合</returns>
        public IEnumerable<string> GetMongodbFile()
        {
            for (int i = 0; i < FileCollection.Length; i+=1)
            {
                yield return FileCollection[i];
            }
        }
        
        /// <summary>
        /// 批量生成还原任务
        /// </summary>
        /// <returns>任务字符串集合</returns>
        public IEnumerable<string> GetCommond()
        {
            for (int i = 0; i < FileCollection.Length; i += 1)
            {
                yield return "mongorestore "+FileCollection[i];
            }
        }
    }
}

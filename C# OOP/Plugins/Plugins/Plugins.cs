using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterfaceLibrary;

namespace Plugins
{
    [TestClass]
    public class Plugins
    {
        [TestMethod]
        public void PluginsTest()
        {
           //The solution is designed as a Unit test
            try
            {
                //You can specify any path convenient for you
                Console.WriteLine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\")));
                string filePath = Path.Combine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\")), @"DllFiles\");
                //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                //string extraPart = @"bin\Debug";
                //int indexFirst = filePath.IndexOf(extraPart);
                //filePath = filePath.Remove(indexFirst, extraPart.Length).Insert(indexFirst, @"DllFiles");
                DirectoryInfo directory = new DirectoryInfo(filePath);
                List<FileInfo> dllList = new List<FileInfo>();

                foreach (FileInfo file in directory.EnumerateFiles("*.dll"))
                {
                    dllList.Add(file);
                }

                if(dllList.Count == 0)
                {
                    throw new ArgumentException();
                }

                foreach (FileInfo dll in dllList)
                {
                    Type[] typeList = Assembly.LoadFile(dll.FullName).GetTypes();
                    List<Type> typeListWithInterface = new List<Type>();
                    foreach (Type type in typeList)
                    {
                        if (type.GetInterfaces().Contains(typeof(IInter)))
                        {
                            typeListWithInterface.Add(type);
                        }
                    }

                    List<object> objectList = new List<object>();
                    foreach (Type exClass in typeListWithInterface)
                    {
                        objectList.Add(Activator.CreateInstance(exClass));
                    }
                    //Print the resulting class instances
                    foreach (object obj in objectList)
                    {
                        Console.WriteLine(obj.ToString());
                    }
                }
            }

            catch (ArgumentException error)
            {
                Assert.Fail("Not found any dll files");                
            }
            catch (Exception error)
            {
                Assert.Fail(error.Message);
            }

        }
    }
}

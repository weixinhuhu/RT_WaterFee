/*
 * 项目地址:http://git.oschina.net/ggshihai/DBLib
 * Author:DeepBlue
 * QQ群:257018781
 * Email:xshai@163.com
 * 说明:一些常用的操作类库.
 * 额外说明:东拼西凑的东西,没什么技术含量,爱用不用,用了你不吃亏,用了你不上当,不用你也取不了媳妇...
 * -------------------------------------------------- 
 * -----------我是长长的美丽的善良的分割线-----------
 * -------------------------------------------------- 
 * 我曾以为无惧时光荏苒 如今明白谁都逃不过似水流年
 * --------------------------------------------------  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System
{
    /// <summary>
    /// T(泛型)对象的扩展方法
    /// </summary>
    public static class Tobject
    {
        #region T Clone

        //http://pragmaticcoding.com/index.php/cloning-objects-in-c/
        //http://blog.csdn.net/wang371756299/article/details/8307877

        /// <summary>
        /// Delegate handler that's used to compile the IL to.
        /// (This delegate is standard in .net 3.5)
        /// </summary>
        /// <typeparam name="T1">Parameter Type</typeparam>
        /// <typeparam name="TResult">Return Type</typeparam>
        /// <param name="arg1">Argument</param>
        /// <returns>Result</returns>
        public delegate TResult Func<T1, TResult>(T1 arg1);
        /// <summary>
        /// This dictionary caches the delegates for each 'to-clone' type.
        /// </summary>
        static Dictionary<Type, Delegate> _cachedIL = new Dictionary<Type, Delegate>();

        /// <summary>
        /// <para>以IL方式克隆(复制)该对象</para>
        /// Generic cloning method that clones an object using IL.
        /// Only the first call of a certain type will hold back performance.
        /// After the first call, the compiled IL is executed.
        /// </summary>
        /// <typeparam name="T">Type of object to clone</typeparam>
        /// <param name="myObject">Object to clone</param>
        /// <returns>Cloned object</returns>
        public static T CloneByIL<T>(this T myObject)
        {
            Delegate myExec = null;
            if (!_cachedIL.TryGetValue(typeof(T), out myExec))
            {
                // Create ILGenerator
                System.Reflection.Emit.DynamicMethod dymMethod = new System.Reflection.Emit.DynamicMethod("DoClone", typeof(T), new Type[] { typeof(T) }, true);
                ConstructorInfo cInfo = myObject.GetType().GetConstructor(new Type[] { });

                System.Reflection.Emit.ILGenerator generator = dymMethod.GetILGenerator();

                System.Reflection.Emit.LocalBuilder lbf = generator.DeclareLocal(typeof(T));
                //lbf.SetLocalSymInfo("_temp");

                generator.Emit(System.Reflection.Emit.OpCodes.Newobj, cInfo);
                generator.Emit(System.Reflection.Emit.OpCodes.Stloc_0);
                foreach (FieldInfo field in myObject.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
                {
                    // Load the new object on the eval stack... (currently 1 item on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Ldloc_0);
                    // Load initial object (parameter)          (currently 2 items on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                    // Replace value by field value             (still currently 2 items on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Ldfld, field);
                    // Store the value of the top on the eval stack into the object underneath that value on the value stack.
                    //  (0 items on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Stfld, field);
                }

                // Load new constructed obj on eval stack -> 1 item on stack
                generator.Emit(System.Reflection.Emit.OpCodes.Ldloc_0);
                // Return constructed object.   --> 0 items on stack
                generator.Emit(System.Reflection.Emit.OpCodes.Ret);

                myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));
                _cachedIL.Add(typeof(T), myExec);
            }
            return ((Func<T, T>)myExec)(myObject);
        }


        /// <summary>
        /// 以序列化方式克隆(复制)该对象,注:被复制对象必须是可序列化
        /// </summary> 
        public static T CloneBySerialization<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("被复制对象不是可序列化对象.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            using (System.IO.Stream objectStream = new System.IO.MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
                System.Runtime.Serialization.IFormatter formatter
                    = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, source);
                objectStream.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }

        //public static object CopyObject(object input)
        //{
        //    if (input != null)
        //    {
        //        object result = Activator.CreateInstance(input.GetType());
        //        foreach (FieldInfo field in input.GetType().GetFields(Consts.AppConsts.FullBindingList))
        //        {
        //            if (field.FieldType.GetInterface("IList", false) == null)
        //            {
        //                field.SetValue(result, field.GetValue(input));
        //            }
        //            else
        //            {
        //                IList listObject = (IList)field.GetValue(result);
        //                if (listObject != null)
        //                {
        //                    foreach (object item in ((IList)field.GetValue(input)))
        //                    {
        //                        listObject.Add(CopyObject(item));
        //                    }
        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public static object CloneByReflection(object obj)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(CloneByReflection(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {

                object toret = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                            BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    if (fieldValue == null)
                        continue;
                    field.SetValue(toret, CloneByReflection(fieldValue));
                }
                return toret;
            }
            else
                throw new ArgumentException("Unknown type");
        }

        #endregion

        /// <summary>
        /// 判断该T(泛型)对象是否为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TobjIsNull<T>(this T value)
        {
            return (T)value == null;
        }

        /// <summary>
        /// 判断该T(泛型)对象不为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TobjIsNotNull<T>(this T value)
        {
            return (T)value != null;
        }
    }


    /// <summary>
    /// Base class for cloning an object in C#
    /// http://www.codeproject.com/Articles/3441/Base-class-for-cloning-an-object-in-C
    /// 
    /// BaseObject class is an abstract class for you to derive from.
    /// Every class that will be dirived from this class will support the 
    /// Clone method automaticly.
    ///
    /// The class implements the interface ICloneable and there 
    /// for every object that will be derived 
    ///
    /// from this object will support the ICloneable interface as well.
    /// </summary>

    public abstract class BaseObject : ICloneable
    {
        /// <summary>
        /// Clone the object, and returning a reference to a cloned object.
        /// </summary>
        /// <returns>Reference to the new cloned 
        /// object.</returns>
        public object Clone()
        {
            //First we create an instance of this specific type.
            object newObject = Activator.CreateInstance(this.GetType());

            //We get the array of fields for the new type instance.
            FieldInfo[] fields = newObject.GetType().GetFields();

            int i = 0;

            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                //We query if the fiels support the ICloneable interface.
                Type ICloneType = fi.FieldType.GetInterface("ICloneable", true);

                if (ICloneType != null)
                {
                    //Getting the ICloneable interface from the object.
                    ICloneable IClone = (ICloneable)fi.GetValue(this);

                    //We use the clone method to set the new value to the field.
                    fields[i].SetValue(newObject, IClone.Clone());
                }
                else
                {
                    // If the field doesn't support the ICloneable 
                    // interface then just set it.
                    fields[i].SetValue(newObject, fi.GetValue(this));
                }

                //Now we check if the object support the 
                //IEnumerable interface, so if it does
                //we need to enumerate all its items and check if 
                //they support the ICloneable interface.
                Type IEnumerableType = fi.FieldType.GetInterface
                                ("IEnumerable", true);
                if (IEnumerableType != null)
                {
                    //Get the IEnumerable interface from the field.
                    System.Collections.IEnumerable IEnum = (System.Collections.IEnumerable)fi.GetValue(this);

                    //This version support the IList and the 
                    //IDictionary interfaces to iterate on collections.
                    Type IListType = fields[i].FieldType.GetInterface("IList", true);
                    Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);

                    int j = 0;
                    if (IListType != null)
                    {
                        //Getting the IList interface.
                        System.Collections.IList list = (System.Collections.IList)fields[i].GetValue(newObject);

                        foreach (object obj in IEnum)
                        {
                            //Checking to see if the current item 
                            //support the ICloneable interface.
                            ICloneType = obj.GetType().
                                GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                //If it does support the ICloneable interface, 
                                //we use it to set the clone of
                                //the object in the list.
                                ICloneable clone = (ICloneable)obj;

                                list[j] = clone.Clone();
                            }

                            //NOTE: If the item in the list is not 
                            //support the ICloneable interface then in the 
                            //cloned list this item will be the same 
                            //item as in the original list
                            //(as long as this type is a reference type).

                            j++;
                        }
                    }
                    else if (IDicType != null)
                    {
                        //Getting the dictionary interface.
                        System.Collections.IDictionary dic
                            = (System.Collections.IDictionary)fields[i].GetValue(newObject);
                        j = 0;

                        foreach (System.Collections.DictionaryEntry de in IEnum)
                        {
                            //Checking to see if the item 
                            //support the ICloneable interface.
                            ICloneType = de.Value.GetType().
                                GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                ICloneable clone = (ICloneable)de.Value;

                                dic[de.Key] = clone.Clone();
                            }
                            j++;
                        }
                    }
                }
                i++;
            }
            return newObject;
        }
    }


    #region Object Cloning Using IL in C#
    public class ILPerson
    {
        private int _id;
        private string _name;
        private string _firstName;
        private string _field1, _field2, _field3;

        public ILPerson()
        {
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
    }

    /// <summary>
    /// Object Cloning Using IL in C#
    /// http://whizzodev.blogspot.com/2008/03/object-cloning-using-il-in-c.html
    /// </summary> 
    class ILProgram
    {
        /// <summary>
        /// Delegate handler that's used to compile the IL to.
        /// (This delegate is standard in .net 3.5)
        /// </summary>
        /// <typeparam name="T1">Parameter Type</typeparam>
        /// <typeparam name="TResult">Return Type</typeparam>
        /// <param name="arg1">Argument</param>
        /// <returns>Result</returns>
        public delegate TResult Func<T1, TResult>(T1 arg1);
        /// <summary>
        /// This dictionary caches the delegates for each 'to-clone' type.
        /// </summary>
        static Dictionary<Type, Delegate> _cachedIL = new Dictionary<Type, Delegate>();

        /// <summary>
        /// The Main method that's executed for the test.
        /// </summary>
        /// <param name="args"></param>
        //static void Main(string[] args)
        //{
        //    DoCloningTest(1000);
        //    DoCloningTest(10000);
        //    DoCloningTest(100000);
        //    //Commented because the test takes long ;)
        //    //DoCloningTest(1000000);

        //    Console.ReadKey();
        //}

        /// <summary>
        /// Do the cloning test and printout the results.
        /// </summary>
        /// <param name="count">Number of items to clone</param>
        private static void DoCloningTest(int count)
        {
            /*
            // Create timer class.
            HiPerfTimer timer = new HiPerfTimer();
            double timeElapsedN = 0, timeElapsedR = 0, timeElapsedIL = 0;

            Console.WriteLine("--> Creating {0} objects...", count);
            timer.StartNew();
            List<ILPerson> personsToClone = CreatePersonsList(count);
            timer.Stop();
            ILPerson temp = CloneObjectWithIL(personsToClone[0]);
            temp = null;
            Console.WriteLine("\tCreated objects in {0} seconds", timer.Duration);

            Console.WriteLine("- Cloning Normal...");
            List<ILPerson> clonedPersons = new List<ILPerson>(count);
            timer.StartNew();
            foreach (ILPerson p in personsToClone)
            {
                clonedPersons.Add(CloneNormal(p));
            }
            timer.Stop();
            timeElapsedN = timer.Duration;

            Console.WriteLine("- Cloning IL...");
            clonedPersons = new List<ILPerson>(count);
            timer.StartNew();
            foreach (ILPerson p in personsToClone)
            {
                clonedPersons.Add(CloneObjectWithIL<ILPerson>(p));
            }
            timer.Stop();
            timeElapsedIL = timer.Duration;

            Console.WriteLine("- Cloning Reflection...");
            clonedPersons = new List<ILPerson>(count);
            timer.StartNew();
            foreach (ILPerson p in personsToClone)
            {
                clonedPersons.Add(CloneObjectWithReflection(p));
            }
            timer.Stop();
            timeElapsedR = timer.Duration;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Object count:\t\t{0}", count);
            Console.WriteLine("Cloning Normal:\t\t{0:00.0000} s", timeElapsedN);
            Console.WriteLine("Cloning IL:\t\t{0:00.0000} s", timeElapsedIL);
            Console.WriteLine("Cloning Reflection:\t{0:00.0000} s", timeElapsedR);
            Console.WriteLine("----------------------------------------");
            Console.ResetColor();
             */
        }

        /// <summary>
        /// Create a list of persons with random data and a given number of items.
        /// </summary>
        /// <param name="count">Number of persons to generate</param>
        /// <returns>List of generated persons</returns>
        private static List<ILPerson> CreatePersonsList(int count)
        {
            Random r = new Random(Environment.TickCount);
            List<ILPerson> persons = new List<ILPerson>(count);
            for (int i = 0; i < count; i++)
            {
                ILPerson p = new ILPerson();
                p.ID = r.Next();
                p.Name = string.Concat("Slaets_", r.Next());
                p.FirstName = string.Concat("Filip_", r.Next());
                persons.Add(p);
            }
            return persons;
        }

        /// <summary>
        /// Clone one person object with reflection
        /// </summary>
        /// <param name="p">Person to clone</param>
        /// <returns>Cloned person</returns>
        private static ILPerson CloneObjectWithReflection(ILPerson p)
        {
            // Get all the fields of the type, also the privates.
            FieldInfo[] fis = p.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            // Create a new person object
            ILPerson newPerson = new ILPerson();
            // Loop through all the fields and copy the information from the parameter class
            // to the newPerson class.
            foreach (FieldInfo fi in fis)
            {
                fi.SetValue(newPerson, fi.GetValue(p));
            }
            // Return the cloned object.
            return newPerson;
        }

        /// <summary>
        /// Generic cloning method that clones an object using IL.
        /// Only the first call of a certain type will hold back performance.
        /// After the first call, the compiled IL is executed.
        /// </summary>
        /// <typeparam name="T">Type of object to clone</typeparam>
        /// <param name="myObject">Object to clone</param>
        /// <returns>Cloned object</returns>
        private static T CloneObjectWithIL<T>(T myObject)
        {
            Delegate myExec = null;
            if (!_cachedIL.TryGetValue(typeof(T), out myExec))
            {
                // Create ILGenerator
                System.Reflection.Emit.DynamicMethod dymMethod = new System.Reflection.Emit.DynamicMethod("DoClone", typeof(T), new Type[] { typeof(T) }, true);
                ConstructorInfo cInfo = myObject.GetType().GetConstructor(new Type[] { });

                System.Reflection.Emit.ILGenerator generator = dymMethod.GetILGenerator();

                System.Reflection.Emit.LocalBuilder lbf = generator.DeclareLocal(typeof(T));
                //lbf.SetLocalSymInfo("_temp");

                generator.Emit(System.Reflection.Emit.OpCodes.Newobj, cInfo);
                generator.Emit(System.Reflection.Emit.OpCodes.Stloc_0);
                foreach (FieldInfo field in myObject.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
                {
                    // Load the new object on the eval stack... (currently 1 item on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Ldloc_0);
                    // Load initial object (parameter)          (currently 2 items on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                    // Replace value by field value             (still currently 2 items on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Ldfld, field);
                    // Store the value of the top on the eval stack into the object underneath that value on the value stack.
                    //  (0 items on eval stack)
                    generator.Emit(System.Reflection.Emit.OpCodes.Stfld, field);
                }

                // Load new constructed obj on eval stack -> 1 item on stack
                generator.Emit(System.Reflection.Emit.OpCodes.Ldloc_0);
                // Return constructed object.   --> 0 items on stack
                generator.Emit(System.Reflection.Emit.OpCodes.Ret);

                myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));
                _cachedIL.Add(typeof(T), myExec);
            }
            return ((Func<T, T>)myExec)(myObject);
        }

        /// <summary>
        /// Clone a person object by manually typing the copy statements.
        /// </summary>
        /// <param name="p">Object to clone</param>
        /// <returns>Cloned object</returns>
        private static ILPerson CloneNormal(ILPerson p)
        {
            ILPerson newPerson = new ILPerson();
            newPerson.ID = p.ID;
            newPerson.Name = p.Name;
            newPerson.FirstName = p.FirstName;
            return newPerson;
        }
    } 
    #endregion

}

using System.Collections.Generic;
using UnityEngine;
using UniVue.Model;

namespace UniVueTest
{
    public class ModelTest : MonoBehaviour
    {
        [ContextMenu("TestValueModel")]
        private void TestValueModel()
        {
            ValueModel model = new() ;
            //((IBindableModel)model)
            var ps = model.GetType().GetProperties();
            foreach (var p in ps)
            {
                print($"propertyName={p.Name} propertyValue={p.GetValue(model)}");
            }
            //((IBindableModel)model).NotifyUIUpdate("Name", 1);
        }

        [ContextMenu("TestTypeString")]
        private void TestTypeString()
        {
            print(typeof(float).FullName);
            print(typeof(int).FullName);
            print(typeof(string).FullName);
            print(typeof(bool).FullName);
            print(typeof(Sprite).FullName);
            print(typeof(List<int>).FullName);
            print(typeof(List<float>).FullName);
            print(typeof(List<string>).FullName);
            print(typeof(List<bool>).FullName);
            print(typeof(List<Sprite>).FullName);
        }
    }

    public partial struct ValueModel 
    {
        [AutoNotify] private string _name;
    }
}

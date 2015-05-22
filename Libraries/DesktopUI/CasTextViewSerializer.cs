using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace DesktopUI
{
    class CasTextViewSerializer
    {
        public CasTextViewSerializer()
        {

        }

        // A method for serializing the content of a textview buffer, allowing it to be saved in formatted form.
        // Is converted to Base64 to avoid problems with JSON.
        public string SerializeCasTextView(CasTextView serializableTextView)
        {
            TextIter startIter, endIter;
            serializableTextView.Buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = serializableTextView.Buffer.Serialize(serializableTextView.Buffer, serializableTextView.Buffer.RegisterSerializeTagset(null), startIter, endIter);
            string serializedTextView = Convert.ToBase64String(byteTextView);

            Console.WriteLine(serializableTextView.Buffer.Text);

            return serializedTextView;
        }

        // A method for deserializing the content serialized in the above method.
        public string DeserializeCasTextView(string serializedTextView)
        {
            CasTextView deserializableTextView = new CasTextView("", false);
            TextIter textIter = deserializableTextView.Buffer.StartIter;
            byte[] byteTextView = Convert.FromBase64String(serializedTextView);
            deserializableTextView.Buffer.Deserialize(deserializableTextView.Buffer, deserializableTextView.Buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);

            Console.WriteLine(deserializableTextView.Buffer.Text);

            return deserializableTextView.Buffer.Text;
        }
    }
}

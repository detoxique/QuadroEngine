using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuadroEngine
{
    [Serializable]
    public class Level : Transformable, Drawable
    {
        public string Name;
        public List<GameObject> objects = new List<GameObject>();

        /// <summary>
        /// Level save method
        /// </summary>
        public void Save()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"levels\" + Name + ".lvl", FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, this);
            stream.Close();
        }

        /// <summary>
        /// Loading level from file
        /// </summary>
        /// <param name="Path">File path</param>
        public void Load(string Path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"levels\" + Name + ".lvl", FileMode.Open, FileAccess.Read);

            formatter.Deserialize(stream);
            stream.Close();
        }

        /// <summary>
        /// Draw level method
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (GameObject go in objects)
            {
                target.Draw(go);
            }
        }
    }
}

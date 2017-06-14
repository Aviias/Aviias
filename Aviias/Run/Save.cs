using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Save
    {
        Map _map;
        Player _player;

        public Save(Map map, Player player)
        {
            _map = map;
            _player = player;
        }

        public Map DeserializeMap()
        {

            Map test;

            FileStream fs = new FileStream("save_map.bin", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            test = (Map)formatter.Deserialize(fs);

            fs.Close();
            return test;
        }

        public void SerializeMap()
        {
            FileStream fs = new FileStream("save_map.bin", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _map);
            fs.Flush();
            fs.Close();
        }

        public void SerializePlayer()
        {
            FileStream fs = new FileStream("save_player.bin", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _player);
            fs.Flush();
            fs.Close();
        }

        public Player DeserializePlayer()
        {

            Player player;

            FileStream fs = new FileStream("save_player.bin", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            player = (Player)formatter.Deserialize(fs);

            fs.Close();
            return player;
        }
    }
}

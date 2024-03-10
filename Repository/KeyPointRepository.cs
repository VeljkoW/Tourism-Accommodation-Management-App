using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class KeyPointRepository
    {

        private const string FilePath = "../../../Resources/Data/keypoints.csv";

        private readonly Serializer<KeyPoint> _serializer;

        private List<KeyPoint> _keypoints;
        public KeyPointRepository()
        {
            _serializer = new Serializer<KeyPoint>();
            _keypoints = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _keypoints = _serializer.FromCSV(FilePath);
            if (_keypoints.Count < 1)
            {
                return 1;
            }
            return _keypoints.Max(c => c.Id) + 1;
        }
        internal void Add(KeyPoint newKeyPoint)
        {
            newKeyPoint.Id = NextId();
            _keypoints.Add(newKeyPoint);
            _serializer.ToCSV(FilePath, _keypoints);
        }
        public List<KeyPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public KeyPoint? GetById(int Id)
        {
            _keypoints = _serializer.FromCSV(FilePath);
            return _keypoints.Find(c => c.Id == Id);
        }
    }
}

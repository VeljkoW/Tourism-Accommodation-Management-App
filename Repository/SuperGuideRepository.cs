using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    internal class SuperGuideRepository: ISuperGuideRepository
    {
        public static SuperGuideRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<SuperGuideRepository>();
        }
        private const string FilePath = "../../../Resources/Data/superGuides.csv";

        private readonly Serializer<SuperGuide> _serializer;

        private List<SuperGuide> _guides;
        public SuperGuideRepository()
        {
            _serializer = new Serializer<SuperGuide>();
            _guides = _serializer.FromCSV(FilePath);
        }
        public SuperGuide? Add(SuperGuide superGuide)
        {
            SuperGuide guide = _guides.Where(t => (t.id == superGuide.id && superGuide.language.Equals(t.language))).FirstOrDefault();
            if (guide != null)
            {
                return superGuide;
            }
            _guides.Add(superGuide);
            _serializer.ToCSV(FilePath, _guides);
            return superGuide;
        }
        public List<SuperGuide> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public bool Delete(SuperGuide superGuide)
        {
            var guideToRemove = _guides
                .FirstOrDefault(t => t.id == superGuide.id && superGuide.language.Equals(t.language));

            if (guideToRemove != null)
            {
                _guides.Remove(guideToRemove);
                _serializer.ToCSV(FilePath, _guides);
                return true;
            }
            return false;
        }
    }
}

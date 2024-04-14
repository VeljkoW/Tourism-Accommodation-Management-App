using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.View.Guest.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class OwnerRepository : IOwnerRepository
    {
        public static OwnerRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerRepository>();
        }
        private const string FilePath = "../../../Resources/Data/owner.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> _owners;
        public OwnerRepository()
        {
            _serializer = new Serializer<Owner>();
            _owners = _serializer.FromCSV(FilePath);
        }

        public void Add(Owner newOwner)
        {
            _owners.Add(newOwner);
            _serializer.ToCSV(FilePath, _owners);
        }

        public List<Owner> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Owner? GetById(int Id)
        {
            _owners = _serializer.FromCSV(FilePath);
            return _owners.Find(c => c.Id == Id);
        }

        public void Update(int Id, bool IsSuperOwner)
        {
            _owners = _serializer.FromCSV(FilePath);
            Owner? Owner = _owners.Find(c => c.Id == Id);
            Owner.IsSuperOwner = IsSuperOwner;
            _serializer.ToCSV(FilePath, _owners);
        }
    }
}

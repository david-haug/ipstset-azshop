using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ipstset.AzShop.Domain.Categories;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Domain.Shops.Events;
using Xunit;
namespace Ipstset.AzShop.Domain.Tests.Shops
{
    public class ShopShould
    {
        [Fact]
        public void Create_Given_Valid_Arguments()
        {
            //arrange
            var name = "name";

            //act
            var sut = Shop.Create(name);

            //Assert
            Assert.True(sut.Id != Guid.Empty);
            Assert.Equal(name, sut.Name);
        }

        [Fact]
        public void Load_Given_Valid_Arguments()
        {
            //arrange
            var id = Guid.NewGuid();
            var name = "name";
           
            //act
            var sut = Shop.Load(id, name);

            //Assert
            Assert.Equal(id, sut.Id);
            Assert.Equal(name, sut.Name);
        }

        [Fact]
        public void Add_ShopCreated_Event()
        {
            var name = "name";

            var sut = Shop.Create(name);
            var events = sut.DequeueEvents();
            var @event = (ShopCreated)events.FirstOrDefault(e => e is ShopCreated);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.ShopId != Guid.Empty);
            Assert.Equal(name, @event.Name);
        }

        [Fact]
        public void Throw_ArgumentException_Given_No_Name()
        {
            var ex = Assert.Throws<ArgumentException>(() => Shop.Create(""));
            Assert.Equal("name", ex.Message);

            //changing name
            var shop = Shop.Load(Guid.NewGuid(), "name");
            var ex2 = Assert.Throws<ArgumentException>(() => shop.ChangeName(""));
            Assert.Equal("name", ex2.Message);
        }

        [Fact]
        public void Change_Name()
        {
            //arrange
            var id = Guid.NewGuid();
            var oldName = "old_name";

            //act
            var sut = Shop.Load(id, oldName);

            var newName = "new_name";
            sut.ChangeName(newName);

            //Assert
            Assert.NotEqual(oldName, sut.Name);
            Assert.Equal(newName, sut.Name);
        }

        [Fact]
        public void Add_ShopNameChanged_Event()
        {
            var name = "name";

            var sut = Shop.Load(Guid.NewGuid(), "old_name");
            sut.ChangeName(name);
            var events = sut.DequeueEvents();
            var @event = (ShopNameChanged)events.FirstOrDefault(e => e is ShopNameChanged);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.ShopId != Guid.Empty);
            Assert.Equal(name, @event.Name);
        }

    }
}

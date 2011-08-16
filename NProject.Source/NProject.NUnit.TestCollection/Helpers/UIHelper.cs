using System.Linq;
using NProject.Helpers;
using NUnit.Framework;

namespace NProject.NUnit.TestCollection.Helpers
{
    class UIHelper_Tests
    {
        private enum SomeEnum
        {
            ValueOne = 1,
            ValueTwo = 2
        }
        [Test]
        public void UIHelper_Creates_SelectList_From_Enum()
        {
            var list = UIHelper.CreateSelectListFromEnum<SomeEnum>();

            //we have two values in enum
            Assert.AreEqual(2, list.Count());
            //text in list element must be same as string representation of enum value
            Assert.AreEqual(SomeEnum.ValueOne.ToString(), list.ElementAt(0).Text);
            //value in list element must be same as int value of enum
            Assert.AreEqual((int) SomeEnum.ValueOne, int.Parse(list.ElementAt(0).Value));
        }
        [Test]
        public void UIHelper_Creates_SelectList_From_Enum_With_Selected_Element()
        {
            var list = UIHelper.CreateSelectListFromEnum<SomeEnum>(SomeEnum.ValueTwo);

            //we have only one selected item in list
            Assert.AreEqual(1, list.Count(i => i.Selected));

            //selected text must be ValueTwo
            Assert.AreEqual(SomeEnum.ValueTwo.ToString(), list.First(i => i.Selected).Text);

            //selected value must be 2
            Assert.AreEqual((int)SomeEnum.ValueTwo, int.Parse(list.First(i => i.Selected).Value));
        }
    }
}

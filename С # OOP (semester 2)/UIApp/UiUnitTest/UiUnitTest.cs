using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.Factory;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace UiUnitTest
{
    [TestClass]
    public class UiUnitTest
    { 
        //download TestStack.White from NuGet
        // WPF.exe and WinForms.exe added to links
        [TestMethod]
        public void WPFUnitTest()
        {
            Application app = Application.Launch("WPF.exe");
          
            Window window = app.GetWindow("Curve", InitializeOption.NoCache);
            Slider slider = window.Get<Slider>("slider");
            Button button = window.Get<Button>("drawButton");
            Button buttonPlus = window.Get<Button>("buttonPlus");
            Button buttonMinus = window.Get<Button>("buttonMinus");
            Label label = window.Get<Label>("currentScale");

            slider.Value = 0.7f;
            Assert.AreEqual(0.7f, Convert.ToSingle(label.Text));

            slider.Value = 2.9f;
            Assert.AreEqual(2.9f, Convert.ToSingle(label.Text));

            slider.Value = 0.2f;
            Assert.AreEqual(0.2f, Convert.ToSingle(label.Text));

            buttonPlus.Click();
            Assert.AreEqual(0.3f, Convert.ToSingle(label.Text));

            buttonMinus.Click();
            Assert.AreEqual(0.2f, Convert.ToSingle(label.Text));

            ComboBox comboBox = window.Get<ComboBox>("curvesComboBox");
            comboBox.Select(0);
            slider.Value = 1f;
            Assert.AreEqual(1f, Convert.ToSingle(label.Text));
            button.Click();
            slider.Value = 3f;
            Assert.AreEqual(3f, Convert.ToSingle(label.Text));
            button.Click();

            comboBox.Select(1);
            slider.Value = 1.2f;
            Assert.AreEqual(1.2f, Convert.ToSingle(label.Text));
            button.Click();
            slider.Value = 0.1f;
            Assert.AreEqual(0.1f, Convert.ToSingle(label.Text));
            button.Click();

            comboBox.Select(2);
            slider.Value = 1.8f;
            Assert.AreEqual(1.8f, Convert.ToSingle(label.Text));
            button.Click();
            buttonPlus.Click();
            Assert.AreEqual(1.9f, Convert.ToSingle(label.Text));
            button.Click();

            comboBox.Select(3);
            buttonMinus.Click();
            Assert.AreEqual(1.8f, Convert.ToSingle(label.Text));
            button.Click();
            slider.Value = 0.9f;
            Assert.AreEqual(0.9f, Convert.ToSingle(label.Text));
            button.Click();

            comboBox.Select(4);
            slider.Value = 1f;
            Assert.AreEqual(1f, Convert.ToSingle(label.Text));
            button.Click();
            slider.Value = 2f;
            Assert.AreEqual(2f, Convert.ToSingle(label.Text));
            button.Click();

            app.Close();

        }

        [TestMethod]
        public void WinFormsUnitTest()
        {
            Application app = Application.Launch("WinForms.exe");

            //can't to use trackbar, because it’s not in TestStack.White
            Window window = app.GetWindow("Curve", InitializeOption.NoCache);
           
            Button button = window.Get<Button>("drawButton");
            Button buttonPlus = window.Get<Button>("buttonPlus");
            Button buttonMinus = window.Get<Button>("buttonMinus");
            Label label = window.Get<Label>("currentScale");

            buttonPlus.Click();
            Assert.AreEqual(1.1f, Convert.ToSingle(label.Text));

            buttonPlus.Click();
            Assert.AreEqual(1.2f, Convert.ToSingle(label.Text));

            buttonMinus.Click();
            Assert.AreEqual(1.1f, Convert.ToSingle(label.Text));

            buttonMinus.Click();
            Assert.AreEqual(1f, Convert.ToSingle(label.Text));

            ComboBox comboBox = window.Get<ComboBox>("curvesComboBox");
            comboBox.Select(0);
            for (var i = 1; i < 7; i++) 
            {
                buttonMinus.Click();
                button.Click();
            }

            comboBox.Select(1);
            for (var i = 1; i < 15; i++)
            {
                buttonPlus.Click();
                button.Click();
            }
          
            comboBox.Select(2);
            for (var i = 1; i < 10; i++)
            {
                buttonMinus.Click();
            }
            button.Click();
            buttonPlus.Click();
            button.Click();

            comboBox.Select(3);
            button.Click();

            comboBox.Select(4);
            button.Click();

            app.Close();
        }
    }
}

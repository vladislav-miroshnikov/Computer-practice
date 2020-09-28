using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;
using Blackjack;

namespace IoС_Container
{
    [TestClass]
    //Unity was loaded from NuGet
    public class IocContainerClass
    {
        [TestMethod]
        public void IocContainerTest()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<AbstractMan, BotFisrt>("BotFirst", new InjectionConstructor(1000,0, new Deck()));
            container.RegisterType<AbstractMan, BotSecond>("BotSecond", new InjectionConstructor(1000, 0, new Deck()));
            container.RegisterType<Deck, Deck>("Deck", new InjectionConstructor());
            Deck myDeck = container.Resolve<Deck>("Deck");
            myDeck.CreateCards();
            BotFisrt botFirst = (BotFisrt)container.Resolve<AbstractMan>("BotFirst");
            BotSecond botSecond = (BotSecond)container.Resolve<AbstractMan>("BotSecond");
            myDeck.Game(botFirst, botSecond);
            botFirst.Info();
            botSecond.Info();
        }
    }
}

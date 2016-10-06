﻿

namespace SmiteQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LeagueSharp;
    using LeagueSharp.Common;


    internal class Program
    {


        private static List<Spell> SpellList = new List<Spell>();

        private static Spell Q;

        private static Spell smite;

        private static Menu Config;

        public static double damage;

        public static string Champ;

        private static int Plevel;

        public static SpellSlot Smite = ObjectManager.Player.GetSpellSlot("SummonerSmite");

       

        private static void Main(string[] kappa)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {

            
             if (ObjectManager.Player.CharData.BaseSkinName == "Amumu")
            {
                Q = new Spell(SpellSlot.Q, 1100);
                Q.SetSkillshot(0.5f, 80f, 2000f, true, SkillshotType.SkillshotLine);
                Champ = "Amumu";
            }
            else if (ObjectManager.Player.CharData.BaseSkinName == "Blitzcrank")
            {
                Q = new Spell(SpellSlot.Q, 1000);
                Q.SetSkillshot(0.5f, 70f, 1800f, true, SkillshotType.SkillshotLine);
                Champ = "Blitzcrank";
            }
            else if (ObjectManager.Player.CharData.BaseSkinName == "LeeSin")
            {
                Q = new Spell(SpellSlot.Q, 1100);
                Q.SetSkillshot(0.5f, 60f, 1500f, true, SkillshotType.SkillshotLine);
                Champ = "LeeSin";
            }
             else if (ObjectManager.Player.CharData.BaseSkinName == "Nidalee")
             {
                 Q = new Spell(SpellSlot.Q, 1500);
                 Q.SetSkillshot(0.5f, 40f, 1300f, true, SkillshotType.SkillshotLine);
                 Champ = "Nidalee";
             }
           /* else if (ObjectManager.Player.CharData.BaseSkinName == "Ivern")
             {
                 Q = new Spell(SpellSlot.Q, 1500);
                 Q.SetSkillshot(0.5f, 40f, 1300f, true, SkillshotType.SkillshotLine);
                 Champ = "Ivern";
             } */
            else
            {
                return;
            }

            smite = new Spell(Smite, 500);

            Config = new Menu("SmiteQ", "SmiteQ", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            Config.AddItem(new MenuItem("qSmite", "Q->Smite")).SetValue(new KeyBind(32, KeyBindType.Press));

            Config.AddToMainMenu();
            int level = ObjectManager.Player.Level;
            Plevel = level;
            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.PrintChat("<font size='20'>SmiteQ by: LordZEDith</font>\n <font color='#b756c5'>Champion selected: </font> " + Champ);
            
        }

        private static void OnGameUpdate(EventArgs args)
        {
            smiteDmg();
            if (ObjectManager.Player.Level > Plevel)
            {
                Plevel = ObjectManager.Player.Level;
                smiteDmg();
            }

            if (Config.Item("qSmite").GetValue<KeyBind>().Active)
            {
                smiteQ();
            }

        }

        public static void smiteQ()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            var prediction = Q.GetPrediction(target);

            if (ObjectManager.Player.IsDead) return;

            var pos1 = Drawing.WorldToScreen(ObjectManager.Player.Position);
            var pos2 = Drawing.WorldToScreen(target.Position);

            if (Config.Item("qSmite").GetValue<KeyBind>().Active)
            {

                smiteDmg();

                if (target == null) return;

                var state = Q.Cast(target);
                if (state.IsCasted())
                {
                    return;
                }

                if (state == Spell.CastStates.Collision)
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.CollisionObjects.Count(i => i.IsValid<Obj_AI_Minion>() && i.IsEnemy) == 1)
                    {
                        if (ObjectManager.Player.Distance(pred.CollisionObjects.First()) > 520)
                        {
                            return;
                        }
                        ObjectManager.Player.Spellbook.CastSpell(smite.Slot, pred.CollisionObjects.First());
                        Q.Cast(pred.CastPosition);
                        return;
                    }
                }
            }
        }

        public static void smiteDmg()
        {
            int level = ObjectManager.Player.Level;
            int[] smitedamage = { 20 * level + 370, 30 * level + 330, 40 * level + 240, 50 * level + 100 };
            damage = smitedamage.Max();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {

            
        }

    }
}

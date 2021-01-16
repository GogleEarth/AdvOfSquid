using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenericScript
{
    public enum ArtifactCategory
    {
        HP_Increase,
        SPEED_Increase,
        ATK_Increase,
        DEF_Increase,
        DMG_Increase
    }

    public enum ArtifactTarget
    {
        Own,
        Enemy,
        Both
    }

    public class ArtifactEffect
    {
        ArtifactTarget Target;
        ArtifactTarget target
        {
            get { return Target; }
            set { Target = value; }
        }


        ArtifactCategory Category;
        public ArtifactCategory category
        {
            get { return Category; }
            set { Category = value; }
        }

        int Value;
        public int value
        {
            get { return Value; }
            set { Value = value; }
        }

        public ArtifactEffect() { }

        public ArtifactEffect(ArtifactTarget t, ArtifactCategory c, int v)
        {
            Target = t;
            Category = c;
            Value = v;
        }

        public string Display()
        {
            return Target + " " + Category + " " + Value + "\n";
        }

    }
    public enum CardTarget
    {
        Own,
        Enemy,
        Both
    }

    public enum CardCategory
    {
        Deal,
        Heal,
        Shield,
        Poison,
        Bleeding,
        Freezing,
        Sleep
    }


    public class CardEffect
    {
        CardTarget Target;
        public CardTarget target 
        {
            get { return Target; }
            set { Target = value; }
        }

        CardCategory Category; 
        public CardCategory category
        {
            get { return Category; }
            set { Category = value; }
        }

        int Value;
        public int value
        {
            get { return Value; } 
            set { Value = value; } 
        }

        public CardEffect() { }

        public CardEffect(CardTarget t, CardCategory c, int v)
        {
            Target = t;
            Category = c;
            Value = v;
        }

        public string Display()
        {
            return Target + " " + Category + " " + Value + "\n";
        }

    }

    public class Artifact
    {
        string artifact_name;
        string image_name;
        string flavor_text;
        string effect_text;
        List<ArtifactEffect> effects;

        public Artifact(string arti_name, string img_name, string flav_text, 
            string eff_text, List<ArtifactEffect> eff)
        {
            artifact_name = arti_name;
            image_name = img_name;
            flavor_text = flav_text;
            effect_text = eff_text;
            effects = eff;
        }

        public string Display()
        {
            string ret_data = artifact_name + "\n" + image_name + "\n" + flavor_text + "\n" + effect_text
                + "\n";

            foreach (ArtifactEffect effect in effects)
            {
                ret_data += effect.Display();
            }

            return ret_data;
        }
    }

    public class Card
    {
        string card_name;
        string image_name;
        string flavor_text;
        string effect_text;
        List<CardEffect> effects;
        public Card(string ca_name, string img_name, string flav_text,
            string eff_text, List<CardEffect> eff)
        {
            card_name = ca_name;
            image_name = img_name;
            flavor_text = flav_text;
            effect_text = eff_text;
            effects = eff;
        }

        public string Display()
        {
            string ret_data = card_name + "\n" + image_name + "\n" + flavor_text + "\n" + effect_text
                + "\n";

            foreach (CardEffect effect in effects)
            {
                 ret_data += effect.Display();
            }
            
            return ret_data;
        }

        public string GetImageName()
        {
            return image_name;
        }

        public string GetEffectText()
        {
            return effect_text;
        }

        public string GetCardName()
        {
            return card_name;
        }
    }

    public enum SkillTarget
    {
        Own,
        Enemy,
        Both
    }

    public enum SkillCategory
    {
        Deal,
        Heal,
        Shield,
        Poison,
        Bleeding,
        Freezing,
        Sleep
    }

    public class SkillEffect
    {
        SkillTarget Target;
        public SkillTarget target
        {
            get { return Target; }
            set { Target = value; }
        }

        SkillCategory Category;
        public SkillCategory category
        {
            get { return Category; }
            set { Category = value; }
        }

        int Value;
        public int value
        {
            get { return Value; }
            set { Value = value; }
        }

        int Cooltime;
        public int cooltime
        {
            get { return Cooltime; }
            set { Cooltime = value; }
        }

        public SkillEffect() { }

        public SkillEffect(SkillTarget t, SkillCategory c, int v, int ct)
        {
            Target = t;
            Category = c;
            Value = v;
            cooltime = ct;
        }

        public string Display()
        {
            return Target + " " + Category + " " + Value + " " + Cooltime + "\n";
        }

    }

    public class Skill
    {
        string mSkillName;
        string mSkillText;
        string mSkillIconName;
        List<SkillEffect> mSkillEffects;

        public string Display()
        {
            string ret_data = mSkillName + "\n" + mSkillText + "\n" + mSkillIconName + "\n";

            foreach (SkillEffect effect in mSkillEffects)
            {
                ret_data += effect.Display();
            }

            return ret_data;
        }
    }

    public class Monster
    {
        string mMonsterName;
        string mSpriteName;
        float mEXP;
        int mLV;
        int mATK;
        int mDEF;
        int mHP;
        float mSPD;
        List<int> mSkills;

        public Monster(string monname, string imgname, 
            float exp, int lv, int atk, int def, int hp, float spd, 
            List<int> skills)
        {
            mMonsterName = monname;
            mSpriteName = imgname;
            mEXP = exp;
            mLV = lv;
            mATK = atk;
            mDEF = def;
            mHP = hp;
            mSPD = spd;
            mSkills = skills;
        }

        public string Display()
        {
            string ret_data = mMonsterName + "\n" + mSpriteName + "\n";

            foreach (int skill in mSkills)
            {
                ret_data += skill + " ,";
            }

            return ret_data;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public class ArtifactEffect
    {
        Target Target;
        Target target
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

        public ArtifactEffect(Target t, ArtifactCategory c, int v)
        {
            Target = t;
            Category = c;
            Value = v;
        }

        public string Display => Target + " " + Category + " " + Value + "\n";

    }
    public enum Target
    {
        Own,
        Enemy,
        Both
    }

    public enum Category
    {
        Deal,
        Heal,
        Shield,
        Poison,
        Bleeding,
        Freezing,
        Sleep
    }

    public class Effect
    {
        Target Target;
        public Target target
        {
            get { return Target; }
            set { Target = value; }
        }

        Category Category;
        public Category category
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

        public Effect() { }

        public Effect(Target t, Category c, int v)
        {
            Target = t;
            Category = c;
            Value = v;
        }

        public string Display => Target + " " + Category + " " + Value + "\n";

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

        public string Display
        {
            get
            {
                string ret_data = artifact_name + "\n" + image_name + "\n" + flavor_text + "\n" + effect_text
                    + "\n";

                foreach (ArtifactEffect effect in effects)
                {
                    ret_data += effect.Display;
                }

                return ret_data;
            }
        }
    }

    public class Card
    {
        string card_name;
        string image_name;
        string flavor_text;
        string effect_text;
        int mCost;
        List<Effect> effects;
        public Card(string ca_name, string img_name, string flav_text,
            string eff_text, int cost, List<Effect> eff)
        {
            card_name = ca_name;
            image_name = img_name;
            flavor_text = flav_text;
            effect_text = eff_text;
            mCost = cost;
            effects = eff;
        }

        public string Display
        {
            get
            {
                string ret_data = card_name + "\n" + image_name + "\n" + flavor_text + "\n" + effect_text
                    + "\n";

                foreach (Effect effect in effects)
                {
                    ret_data += effect.Display;
                }

                return ret_data;
            }
        }

        public string ImageName => image_name;

        public string EffectText => effect_text;

        public string CardName => card_name;

        public List<Effect> CardEffects => effects;

        public int Cost => mCost;
    }

    public class Skill
    {
        string mSkillName;
        string mSkillText;
        string mSkillIconName;
        List<Effect> mSkillEffects;
        int mCooltime;

        public Skill(string skillname, string skilliconname, string skilltext,
            List<Effect> skilleffects, int cooltime)
        {
            mSkillName = skillname;
            mSkillText = skilltext;
            mSkillIconName = skilliconname;
            mSkillEffects = skilleffects;
            mCooltime = cooltime;
        }

        public string Display
        {
            get
            {
                string ret_data = mSkillName + "\n" + mSkillIconName + "\n" + mSkillText + "\n" + mCooltime + "\n";

                foreach (Effect effect in mSkillEffects)
                {
                    ret_data += effect.Display;
                }

                return ret_data;
            }
        }

        public int Cooltime => mCooltime;

        public List<Effect> Effects => mSkillEffects;

        public string SkillIcon => mSkillIconName;

        public string SkillName => mSkillName;
    }

    public class Monster
    {
        string mMonsterName;
        string mSpriteName;
        string mIconName;
        int mEXP;
        int mLV;
        int mATK;
        int mDEF;
        int mHP;
        float mSPD;
        List<int> mSkills;

        public Monster(string monname, string imgname, string iconname,
            int exp, int lv, int atk, int def, int hp, float spd,
            List<int> skills)
        {
            mMonsterName = monname;
            mSpriteName = imgname;
            mIconName = iconname;
            mEXP = exp;
            mLV = lv;
            mATK = atk;
            mDEF = def;
            mHP = hp;
            mSPD = spd;
            mSkills = skills;
        }

        public string Display
        {
            get
            {
                string ret_data = mMonsterName + "\n" + mSpriteName + "\n" + mIconName + "\n";

                foreach (int skill in mSkills)
                {
                    ret_data += skill + " ,";
                }

                return ret_data;
            }
        }

        public int EXP => mEXP;

        public int LV => mLV;

        public int ATK => mATK;

        public int DEF => mDEF;

        public int HP => mHP;

        public float SPD => mSPD;

        public List<int> Skills => mSkills;

        public string SpriteName => mSpriteName;

        public string IconName => mIconName;

    }

    public static class EnumUtil<T>
    {
        public static T Parse(string s)
        {
            return (T)Enum.Parse(typeof(T), s);
        }
    }

    public class Buff
    {
        string mName;
        string mEffectText;
        string mIconName;
        Category mCategory;
        int mValue;
        int mDuration;

        public Buff(string name, string effectText, string iconName,
            Category category, int value, int duration)
        {
            mName = name;
            mEffectText = effectText;
            mIconName = iconName;
            mCategory = category;
            mValue = value;
            mDuration = duration;
        }

        public string Name => mName;

        public string EffectText => mEffectText;

        public string IconName => mIconName;

        public Category BuffCategory => mCategory;

        public int Value => mValue;

        public int Duration => mDuration;



    }

}
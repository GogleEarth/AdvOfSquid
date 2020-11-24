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
        List<ArtifactTarget> targets;
        Dictionary<ArtifactCategory,int> effection;
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

    public enum CardTarget
    {
        Own,
        Enemy,
        Both
    }

    public class CardEffect
    {
        public List<CardTarget> targets;
        public List<Dictionary<CardCategory, int>> effection;

        public CardEffect()
        {
            targets = new List<CardTarget>();
            effection = new List<Dictionary<CardCategory, int>>();
        }

        public string Display()
        {
            string ret_data = " Target : ";

            foreach (CardTarget target in targets)
            {
                ret_data += target;
                ret_data += " ";
            }
            ret_data += "Effection :";
            foreach (Dictionary<CardCategory,int> dic in effection)
            {
                foreach (KeyValuePair<CardCategory, int> data in dic)
                {
                    ret_data += data.Key;
                    ret_data += "-";
                    ret_data += data.Value;
                    ret_data += " ";
                }
            }

            return ret_data;
        }
    }

    public class Artifact
    {
        string artifact_name;
        string image_name;
        string flavor_text;
        string effect_text;
        ArtifactEffect effect;

        public Artifact(string arti_name, string img_name, string flav_text, 
            string eff_text, ArtifactEffect eff)
        {
            artifact_name = arti_name;
            image_name = img_name;
            flavor_text = flav_text;
            effect_text = eff_text;
            effect = eff;
        }
    }

    public class Card
    {
        string card_name;
        string image_name;
        string flavor_text;
        string effect_text;
        CardEffect effect;
        public Card(string ca_name, string img_name, string flav_text,
            string eff_text, CardEffect eff)
        {
            card_name = ca_name;
            image_name = img_name;
            flavor_text = flav_text;
            effect_text = eff_text;
            effect = eff;
        }

        public string Display()
        {
            string ret_data;

            ret_data = card_name + " " + image_name + " " + flavor_text + " " + effect_text
                + "" + effect.Display();

            return ret_data;
        }
    }

}

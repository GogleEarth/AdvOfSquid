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

    public class Artifact
    {
        string artifact_name;
        string image_name;
        string flavor_text;
        string effect_text;
        public GameObject icon;
        ArtifactEffect effect;

    }

    public class Card
    {
        string card_name;
        string image_name;
        string flavor_text;
        string effect_text;
        public GameObject card_image;
        ArtifactEffect effect;
    }

}

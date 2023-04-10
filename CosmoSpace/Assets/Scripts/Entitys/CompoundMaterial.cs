using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CS
{
    [Serializable]
    public class CompoundMaterial
    {
        [SerializeField] private Shader _shader;
        [SerializeField] private List<SimpleMaterial> _compound = new List<SimpleMaterial>();
        [SerializeField] private float _size = 0;

        public UnityEvent<List<SimpleMaterial>> OnCompoundChange = new UnityEvent<List<SimpleMaterial>>();

        public CompoundMaterial ()
        {

        }

        public Material GetMaterial ()
        {
            int compoundCount = this._compound.Count; 

            if (compoundCount > 0)
            {
                List<Material> materials = this._compound.ConvertAll((m) => m.material.texture);
                List<Texture2D> textures = materials.ConvertAll((m) => (Texture2D)m.mainTexture);
                List<Color> colors = materials.ConvertAll((c) => c.color);
                Material material = new Material(this._shader);

                // compound textures
                if (compoundCount > 1)
                {
                    Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
                    texture.anisoLevel = 8;

                    for (int i = 0; i < texture.width; i++)
                    {
                        for (int j = 0; j < texture.height; j++)
                        {
                            int textureNumber = (int)UnityEngine.Random.Range(0, textures.Count);
                            Texture2D compTexture = textures[textureNumber];
                            Color compColor = colors[textureNumber];
                            Color compPixel;

                            if (compTexture != null)
                            {
                                compPixel = (compTexture.GetPixel(i, j) + compColor) / 2;
                            }
                            else
                            {
                                compPixel = compColor - new Color(0, 0, 0, .5f);
                            }

                            texture.SetPixel(i, j, compPixel);
                        }
                    }

                    texture.Apply();

                    material.SetTexture("_MainTex", texture);
                    //material.SetTexture("_BumpMap", this._NormalMap(texture, 1));
                }
                else
                {
                    material.SetTexture("_MainTex", textures[0]);
                    material.SetColor("_Color", colors[0]);
                    //material.SetTexture("_BumpMap", this._NormalMap(textures[0], 1));
                }

                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                //material.DisableKeyword("_ALPHATEST_ON");
                //material.DisableKeyword("_ALPHABLEND_ON");
                //material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.enableInstancing = true;
                material.renderQueue = 3000;
                material.SetFloat("_Mode", 2);

                return material;
            }

            return null;
        }

        private Texture2D _NormalMap(Texture2D source, float strength)
        {
            strength = Mathf.Clamp(strength, 0.0F, 10.0F);
            Texture2D result;
            float xLeft;
            float xRight;
            float yUp;
            float yDown;
            float yDelta;
            float xDelta;
            result = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
            for (int by = 0; by < result.height; by++)
            {
                for (int bx = 0; bx < result.width; bx++)
                {
                    xLeft = source.GetPixel(bx - 1, by).grayscale * strength;
                    xRight = source.GetPixel(bx + 1, by).grayscale * strength;
                    yUp = source.GetPixel(bx, by - 1).grayscale * strength;
                    yDown = source.GetPixel(bx, by + 1).grayscale * strength;
                    xDelta = ((xLeft - xRight) + 1) * 0.5f;
                    yDelta = ((yUp - yDown) + 1) * 0.5f;
                    result.SetPixel(bx, by, new Color(xDelta, yDelta, 1.0f, yDelta));
                }
            }
            result.Apply();
            return result;
        }

        public void AddToCompound (SimpleMaterial material)
        {
            if (this._compound.Contains(material))
            {
                this._compound.Find(x => x == material).amount += material.amount;
            }
            else
            {
                this._compound.Add(material);
            }

            this._size += material.amount;
            this.OnCompoundChange.Invoke(this._compound);
        }

        public SimpleMaterial TakeFromCompound (SimpleMaterial material, float amount = -1)
        {
            SimpleMaterial compoundMaterial = this._compound.Find(x => x == material);

            if (amount == -1)
            {
                if (compoundMaterial != null)
                {
                    this._compound.Remove(compoundMaterial);
                    this._size -= compoundMaterial.amount;
                    this.OnCompoundChange.Invoke(this._compound);
                    return new SimpleMaterial(material.material, compoundMaterial.amount);
                }
                else
                {
                    return null;
                }
            }
            else if (amount <= compoundMaterial.amount)
            {
                compoundMaterial.amount -= amount;
                this._size -= material.amount;
                this.OnCompoundChange.Invoke(this._compound);
                return new SimpleMaterial(material.material, amount);
            }
            else
            {
                return null;
            }
        }
    }
}
//================================================================================================================================
//
//  Copyright (c) 2015-2019 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;
using UnityEngine.Events;
using PolyToolkit;

namespace easyar
{
    public abstract class TargetController : MonoBehaviour
    {

        string urlAsset;
        private GameObject marcador;
        private GameObject[] modeloPoly;
        int primeraSeleccion = 0;

        int valor = 0;


        public ActiveControlStrategy ActiveControl;
        public bool HorizontalFlip;

        private bool firstFound;

        public UnityEvent TargetFound;
        public UnityEvent TargetLost;

        public enum ActiveControlStrategy
        {
            HideWhenNotTracking,
            HideBeforeFirstFound,
            None,
        }

        public bool IsTracked { get; private set; }

        protected virtual void Start()
        {
            if (!IsTracked && (ActiveControl == ActiveControlStrategy.HideWhenNotTracking || ActiveControl == ActiveControlStrategy.HideBeforeFirstFound))
            {
                ActivarRenderizador(false);
   
            }
        }


        public void ObtenerAsset(string assetUrl)
        {
           
             switch (primeraSeleccion)
             {
                 case 0:


                     urlAsset = assetUrl;
                     primeraSeleccion = 1;
                     break;

                 case 1:

                     modeloPoly = GameObject.FindGameObjectsWithTag("Player");
                     for (int i = 0; i < modeloPoly.Length; i++)
                     {
                         Destroy(modeloPoly[i].gameObject);
                     }

                     urlAsset = assetUrl;
                     Debug.Log("Segunda entrada");
                    IsTracked = false;
                    //PolyApi.GetAsset(urlAsset, MyCallback);
                    break;
             }



        }



        internal void OnTracking(bool status)
        {
            if (IsTracked != status)
            {
                if (status)
                {
                   
                    if (ActiveControl == ActiveControlStrategy.HideWhenNotTracking || (ActiveControl == ActiveControlStrategy.HideBeforeFirstFound && !firstFound))
                    {
                        Debug.Log(status);
                        ActivarRenderizador(true);
                        Debug.Log("Primera entrada");
                        PolyApi.GetAsset(urlAsset, MyCallback);
                        
                    }
                    firstFound = true;
                    if (TargetFound != null)
                    {
                        TargetFound.Invoke();
                    }
                }
                else
                {
                    if (ActiveControl == ActiveControlStrategy.HideWhenNotTracking)
                    {
                        Debug.Log(status);
                        ActivarRenderizador(false);
                     
                        modeloPoly = GameObject.FindGameObjectsWithTag("Player");
                        for (int i = 0; i < modeloPoly.Length; i++)
                        {
                            Destroy(modeloPoly[i].gameObject);
                        }
                    }
                    if (TargetLost != null)
                    {
                        TargetLost.Invoke();
                    }
                }
                IsTracked = status;
            }
            if (IsTracked)
            {
                OnTracking();
            }
        }

        protected abstract void OnTracking();


        void MyCallback(PolyStatusOr<PolyAsset> result)
        {
            if (!result.Ok)
            {
                // Handle error.
                Debug.LogError("Error al obtener el asset. Razon: " + result.Status);
                return;
            }
            // Success. result.Value is a PolyAsset
            // Do something with the asset here.

            PolyImportOptions options = PolyImportOptions.Default();

            options.rescalingMode = PolyImportOptions.RescalingMode.FIT;

            options.desiredSize = 1.0f;

            options.recenter = true;


            Debug.Log("Asset obtenido exitosamente: " + result.Value);

            PolyApi.Import(result.Value, options, ImportarAsset);

        }

        void ImportarAsset(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
        {
          

           modeloPoly = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < modeloPoly.Length; i++)
            {
                Destroy(modeloPoly[i].gameObject);
            }



            if (!result.Ok)
            {
                // Handle error.
                Debug.LogError("Error al importar el asset. Razon: " + result.Status);
                return;
            }
            // Success. Place the result.Value.gameObject in your scene.
            Debug.Log("Asset importado exitosamente");

            marcador = GameObject.Find("ImageTarget");
            // result.Value.gameObject.transform.SetParent(marcador.transform, false);
            result.Value.gameObject.transform.parent = marcador.transform;
            result.Value.gameObject.transform.position = new Vector3(0, 0, 0);
            result.Value.gameObject.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
            result.Value.gameObject.tag = "Player";
            
        }

        void ActivarRenderizador(bool active)
        {
            foreach(Renderer rend in GetComponentsInChildren<Renderer>(true))
            {
                rend.enabled = active;
            }
        }

        
    }
}

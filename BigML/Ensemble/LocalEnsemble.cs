﻿using System.Collections.Generic;
using System.Json;


namespace BigML
{
    public partial class Ensemble
    {
        public class LocalEnsemble
        {
            readonly JsonValue _jsonObject;
            readonly JsonArray _modelResourcesIds;
            List<Model.LocalModel> _models = new List<Model.LocalModel>();
            Model.Node[] _modelsPredictions;

            internal LocalEnsemble(JsonValue jsonObject, JsonArray modelIds)
            {
                _jsonObject = jsonObject;
                _modelResourcesIds = modelIds;
                //_models = new Model.LocalModel[_modelResourcesIds.Count];
                _modelsPredictions = new Model.Node[_modelResourcesIds.Count];
            }


            public int addLocalModel(Model.LocalModel localModel)
            {
                this._models.Add(localModel);
                return this._models.Count;
            }

        
            public Dictionary<string, object> predict(Dictionary<string, dynamic> inputData, bool byName = true, int missing_strategy = 0)
            {
                IList<Prediction> outputs = new List<Prediction>();
 
                Dictionary<string, dynamic> dataById = new Dictionary <string, dynamic>();
                var mv = new MultiVote();

                for (int i = 0; i < this._models.Count; i++)
                {
                    _modelsPredictions[i] = this._models[i].predict(inputData);
                    mv.append(_modelsPredictions[i].toDictionary());
                }

                return mv.getGroupedDistribution(mv);
            }
        }
    }
}


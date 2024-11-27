using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplePrices.Models.Repositories
{
    // on définit un modèle de base et on spécifie que
    // TResult soit toujour une classe (donc pas une valeur)
    // Tkey soit toujours une valeur (donc pas une classe)

    interface IRepository<TResult, TKey> where TResult : class where TKey : struct
    {
        IEnumerable<TResult> Get();
        TResult Get(TKey id);
        TResult Post(TResult entity);
        TResult Put(TResult entity);
        bool Delete(TKey id);
    }
}
namespace ProducerGennericDataMigration.Repository;

public class MessageProcessRepository
{
    public async Task<List<BatchSqlModelInput>> process(SqsMessageBodyModel requestMessageDataModel, int batchSize)
    {
        int top = batchSize;
        int skip = 0;
        var BatchLists = new List<BatchSqlModelInput>();
        while ((top + skip) < (requestMessageDataModel.Count - 1))
        {
            var model = new BatchSqlModelInput();
            model.RequestId = requestMessageDataModel.RequestId;
            model.Status = "BatchCreated";
            model.IsLastBatch = false;
            model.top = top;
            model.Skip = skip;
            
            BatchLists.Add(model);
            skip += top;
        }
        var Lastmodel = new BatchSqlModelInput();
        Lastmodel.RequestId = requestMessageDataModel.RequestId;
        Lastmodel.Status = "BatchCreated";
        Lastmodel.IsLastBatch = true;
        Lastmodel.top = requestMessageDataModel.Count-skip;
        Lastmodel.Skip = skip;
        
        BatchLists.Add(Lastmodel);
        return BatchLists;
    }
}
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FinalProject.Models;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Version = Lucene.Net.Util.Version;
using Lucene.Net.QueryParsers;

namespace FinalProject.LuceneSearch
{
    public class PostcardSearcher
    {
        private static string lucenePath = Path.Combine(HttpRuntime.AppDomainAppPath,
            "postcard_index");

        private static FSDirectory postcardDirectory;

        private static FSDirectory PostcardDirectory
        {
            get
            {
                if (postcardDirectory == null)
                {
                    postcardDirectory = FSDirectory.Open(new DirectoryInfo(lucenePath));
                }
                if (IndexWriter.IsLocked(postcardDirectory))
                {
                    IndexWriter.Unlock(postcardDirectory);
                }
                var lockFilePath = Path.Combine(lucenePath, "write.lock");
                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }
                return postcardDirectory;
            }
        }

        private static void AddToLuceneIndex(Postcard postcard, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term("Id", postcard.Id.ToString()));
            writer.DeleteDocuments(searchQuery);
            var doc = new Document();
            doc.Add(new Field("Id", postcard.Id.ToString(), Field.Store.YES,
                Field.Index.NOT_ANALYZED));
            doc.Add(new Field("OwnerId", postcard.OwnerId.ToString(), Field.Store.YES,
                Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", postcard.Name, Field.Store.YES,
                Field.Index.ANALYZED));
            doc.Add(new Field("ImageUrl", postcard.ImageUrl, Field.Store.YES,
                Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ThumbnailUrl", postcard.ThumbnailUrl, Field.Store.YES,
                Field.Index.NOT_ANALYZED));
            writer.AddDocument(doc);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<Postcard> postcards)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(PostcardDirectory, analyzer,
                IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var postcard in postcards)
                {
                    AddToLuceneIndex(postcard, writer);
                }
                analyzer.Close();
            }
        }

        public static void AddUpdateLuceneIndex(Postcard postcard)
        {
            AddUpdateLuceneIndex(new List<Postcard> { postcard });
        }

        public static void ClearIndexRecord(int postcardId)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(PostcardDirectory, analyzer,
                IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("Id", postcardId.ToString()));
                writer.DeleteDocuments(searchQuery);
                analyzer.Close();
            }
        }

        public static bool ClearIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(PostcardDirectory, analyzer,
                    true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                    analyzer.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(PostcardDirectory, analyzer,
                IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
            }
        }

        private static Postcard MapLuceneDocumentToData(Document doc)
        {
            return new Postcard
            {
                Id = Convert.ToInt32(doc.Get("Id")),
                OwnerId = doc.Get("OwnerId"),
                Name = doc.Get("Name"),
                ImageUrl = doc.Get("ImageUrl"),
                ThumbnailUrl = doc.Get("ThumbnailUrl")
            };
        }

        private static IEnumerable<Postcard> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<Postcard> MapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(
                hit.Doc))).ToList();
        }

        private static Query ParseQuery (string searchQuery, QueryParser parser)
        {
            Query query;
            searchQuery = searchQuery.Trim();
            try
            {
                query = parser.Parse(searchQuery);
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery));
            }
            return query;
        }

        public static IEnumerable<Postcard> Search(string searchQuery, 
            string searchField, int hitsLimit = 1000)
        {
            if (String.IsNullOrEmpty(searchQuery) || String.IsNullOrEmpty(searchField))
            {
                return new List<Postcard>();
            }
            IEnumerable<Postcard> results = null;
            using (var searcher = new IndexSearcher(PostcardDirectory, false))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                var query = ParseQuery(searchQuery, parser);
                var hits = searcher.Search(query, hitsLimit).ScoreDocs;
                results = MapLuceneToDataList(hits, searcher);
                analyzer.Close();
            }
            return results;
        }
    }
}
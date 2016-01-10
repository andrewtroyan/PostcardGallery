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
    public class HashTagSearcher
    {
        private static string lucenePath = Path.Combine(HttpRuntime.AppDomainAppPath,
            "hashtag_index");

        private static FSDirectory hashtagDirectory;

        private static FSDirectory HashtagDirectory
        {
            get
            {
                if (hashtagDirectory == null)
                {
                    hashtagDirectory = FSDirectory.Open(new DirectoryInfo(lucenePath));
                }
                if (IndexWriter.IsLocked(hashtagDirectory))
                {
                    IndexWriter.Unlock(hashtagDirectory);
                }
                var lockFilePath = Path.Combine(lucenePath, "write.lock");
                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }
                return hashtagDirectory;
            }
        }

        private static void AddToLuceneIndex(HashTag hashTag, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term("Id", hashTag.Id.ToString()));
            writer.DeleteDocuments(searchQuery);
            var doc = new Document();
            doc.Add(new Field("Id", hashTag.Id.ToString(), Field.Store.YES,
                Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Value", hashTag.Value, Field.Store.YES,
                Field.Index.ANALYZED));
            writer.AddDocument(doc);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<HashTag> hashTags)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(HashtagDirectory, analyzer,
                IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var hashTag in hashTags)
                {
                    AddToLuceneIndex(hashTag, writer);
                }
                analyzer.Close();
            }
        }

        public static void AddUpdateLuceneIndex(HashTag hashTag)
        {
            AddUpdateLuceneIndex(new List<HashTag> { hashTag });
        }

        public static void ClearIndexRecord(int hashTagId)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(HashtagDirectory, analyzer,
                IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("Id", hashTagId.ToString()));
                writer.DeleteDocuments(searchQuery);
                analyzer.Close();
            }
        }

        public static bool ClearIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(HashtagDirectory, analyzer,
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
            using (var writer = new IndexWriter(HashtagDirectory, analyzer,
                IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
            }
        }

        private static HashTag MapLuceneDocumentToData(Document doc)
        {
            return new HashTag
            {
                Id = Convert.ToInt32(doc.Get("Id")),
                Value = doc.Get("Value")
            };
        }

        private static IEnumerable<HashTag> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<HashTag> MapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(
                hit.Doc))).ToList();
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            searchQuery = searchQuery.Trim().ToLower() + "*";
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

        public static IEnumerable<HashTag> Search(string searchQuery,
            string searchField, int hitsLimit = 1000)
        {
            if (String.IsNullOrEmpty(searchQuery) || String.IsNullOrEmpty(searchField))
            {
                return new List<HashTag>();
            }
            IEnumerable<HashTag> results = null;
            using (var searcher = new IndexSearcher(HashtagDirectory, false))
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
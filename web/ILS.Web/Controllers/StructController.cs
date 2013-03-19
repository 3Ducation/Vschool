﻿using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ILS.Domain;
using ILS.Web.Extensions;

using System.Web.Script.Serialization;

using System.Data.Objects;
using System.Data.Entity.Infrastructure;
//using ziparciv = Ionic.Zip;
//using HtmlAgilityPack;

namespace ILS.Web.Controllers
{
	[Authorize(Roles = "Admin, Teacher")]
    public class StructController : JsonController
	{
		ILSContext context;
        ObjectContext context_obj;

		public StructController(ILSContext context)
		{
			this.context = context;
            this.context_obj = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
		}
		
		//[Authorize(Roles = "Teacher")]
		public ActionResult Index()
		{
			return View();
		}

        //[Authorize(Roles = "Teacher")]
        /*public string AddDoc(Guid id, string type_file, HttpPostedFileBase file)
        {
            if (!ziparciv.ZipFile.IsZipFile(file.InputStream,true)){
                return "{\"success\":false}";
            }
            file.InputStream.Position = 0;
            //var zipFileToRead = file.FileName;
            var p = context.ThemeContent.Find(id);
           // var l = context.ThemeContent.Find(parent_id);
            string path = "/Course_" + p.Theme.Course_Id.ToString();
            path += "/Theme_" + p.Theme_Id.ToString();
            path += "/Lecture_" + p.Theme_Id.ToString();
            string virtpath = HttpContext.Request.Url.ToString().Replace("/Struct/AddDoc", "") + "/Content/pics_base" + path;
            path = Server.MapPath("~/Content/pics_base") + path;

            string extractToFolder = Server.MapPath("~/Content/temp/") + file.FileName.Substring(0, file.FileName.Length - ".zip".Length);

            string folderimages = "";
            string htmFileName = "";
            using (var zip = ziparciv.ZipFile.Read(file.InputStream))   // Read( zipFileToRead))
            {
                foreach (var entry in zip.Entries)
                {
                    entry.Extract(extractToFolder, ziparciv.ExtractExistingFileAction.OverwriteSilently);
                    if (entry.FileName.Substring(entry.FileName.Length - ".htm".Length, ".htm".Length) == ".htm")
                    {
                        htmFileName = extractToFolder + "\\" + entry.FileName;
                        folderimages = entry.FileName.Substring(0,entry.FileName.Length - ".htm".Length) + ".files";
                    }
                }
            }
            
            var l = context.ThemeContent.Find(id);
            int num;
            if (l.Paragraphs.Count == 0) num = 1;
            else num = l.Paragraphs.OrderBy(x => x.OrderNumber).Last().OrderNumber + 1;
            Paragraph par;
          //  l.Paragraphs.Add(par);
         //   context.SaveChanges();
           

            if (type_file == "lecture") {
                HtmlWeb webDoc = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = webDoc.Load(htmFileName);
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//p");//SelectNodes(".//img")
                string srcimg = "";
                string text = "";
                string texttemp = "";
                string path_par = "";
                string virtpath_par = "";
                
                string header = "";
                
                Boolean flag = true;
                Picture[] pics = new Picture[50];
                Picture pic = new Picture();
                int picnum=1;
                if (nodes != null)
                {
                    foreach (var tag in nodes)
                    {
                        if (tag.InnerText != "")
                        {
                            if (tag.GetAttributeValue("class", "") == "vwheader")
                            {
                                
                                if (flag) {
                                    flag = false;
                                }
                                else {
                                    par = new Paragraph { OrderNumber = num, Header = header, Text = text };
                                    l.Paragraphs.Add(par);
                                    context.SaveChanges();
                                    num++;
                                    path_par = path+"/Paragraph_" + par.Id.ToString();
                                    virtpath_par = virtpath + "/Paragraph_" + par.Id.ToString();
                                    for (int i = 0 ; i < picnum-1; i++){
                                        Picture picitem=pics[i];
                                        string file_in = extractToFolder+"/"+picitem.Path;
                                        string file_out = path_par+picitem.Path.Replace(folderimages, "");
                                        if (!Directory.Exists(path_par)) Directory.CreateDirectory(path_par); 
                                        System.IO.File.Copy(file_in, file_out,true);
                                        par.Pictures.Add(new Picture { OrderNumber = picitem.OrderNumber, Path = virtpath_par + picitem.Path.Replace(folderimages, "") });
                                    }
                                    context.SaveChanges();
                                    picnum = 1;
                                    
                                    pics.Initialize();
                                }
                                header = tag.InnerText;
                                text = "";
                                //textBox2.AppendText(Environment.NewLine + Environment.NewLine + "                       " + tag.InnerText + Environment.NewLine + Environment.NewLine);
                            }
                            else if (tag.GetAttributeValue("class", "") == "vwparagraf")
                            {
                                if (!flag)
                                {
                                    texttemp = tag.InnerText;
                                    texttemp = texttemp.Replace("&nbsp;", "");
                                    texttemp = texttemp.Replace("\r\n", "");
                                    text = text + texttemp + "\r\n";
                                }
                                //textBox2.AppendText("    " + text + Environment.NewLine);
                            }

                        }
                        HtmlNodeCollection images = tag.SelectNodes(".//img");
                        if (images != null)
                            if (!flag)                                
                            {
                                foreach (var image in images)
                                {
                                
                                    string src = image.GetAttributeValue("src", "");
                                    if (src != "")
                                    {
                                        pics[picnum - 1] = new Picture();
                                        pics[picnum - 1].OrderNumber = picnum;
                                        pics[picnum - 1].Path = src;  
                                        picnum++;
                                    
                                        //srcimg = srcimg + src + Environment.NewLine;
                                    }
                                }
                            }
                        
                    }
                    //textBox2.AppendText("Список изображений этого абзаца:" + Environment.NewLine + srcimg);
                    ////HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    //// doc.Load(htmFileName);
                    //// foreach(HtmlNode link in doc.DocumentElement.SelectNodes("//a[@href]")
                    //// {
                    ////    HtmlAttribute att = link.Attributes["href"];
                    ////    att.Value = FixLink(att);
                    //// }
                    //// doc.Save("file.htm");
                }
                if (header != "")
                {
                    par = new Paragraph { OrderNumber = num, Header = header, Text = text };
                    l.Paragraphs.Add(par);
                    context.SaveChanges();
                    num++;
                    path_par = path + "/Paragraph_" + par.Id.ToString();
                    virtpath_par = virtpath + "/Paragraph_" + par.Id.ToString();
                    for (int i = 0; i < picnum - 1; i++)
                    {
                        Picture picitem = pics[i];
                        string file_in = extractToFolder + "/" + picitem.Path;
                        string file_out = path_par + picitem.Path.Replace(folderimages, "");
                        if (!Directory.Exists(path_par)) Directory.CreateDirectory(path_par);
                        System.IO.File.Copy(file_in, file_out, true);
                        par.Pictures.Add(new Picture { OrderNumber = picitem.OrderNumber, Path = virtpath_par + picitem.Path.Replace(folderimages, "") });
                        context.SaveChanges();
                    }
                    int ljk = 1;
                }
                return "{\"success\":true}";
            }
            else if (type_file == "test") {

                return "{\"success\":true}";
            }
            else return "{\"success\":false}";
            
        }*/

		public ActionResult ReadTree(string node)        
		{
            /*Метод вызывается деревом из файла struct.js в двух случаях. Первый - при первоначальной загрузке дерева.
            Тогда id = "treeRoot", и мы возвращаем список курсов. Второй - когда пользователь разворачивает очередную ветку дерева.
            Тогда id = идентификатор курса/темы/лекции/теста (поиском по базе данных мы можем узнать, чего именно),
            и нам нужно вернуть список тем этого курса / лекций и тестов этой темы / параграфов этой лекции / вопросов этого теста*/
			if (node == "treeRoot")
			{
				/*верхний уровень - возвращаем список курсов. Заметьте, что оформлен он именно так, как нужно элементам дерева,
                т.е. есть параметр iconCls, определяющий иконку, текст элемента, а также id, с помощью которого мы,
                в свою очередь, сможем развернуть следующую ветку, когда снова вызовем этот метод*/
                return Json(context.Course.ToList().OrderBy(x => x.Name).Select(x => new {
                    iconCls = "course",
					id = x.Id.ToString(),
					text = x.Name
				}), JsonRequestBehavior.AllowGet);
			}
			var guid = Guid.Parse(node);
			var course = context.Course.Find(guid);
			if (course != null)
			{
                //уровень курса - возвращаем список тем
				return Json(course.Themes.ToList().OrderBy(x => x.OrderNumber).Select(x => new {
				    iconCls = "theme",
					id = x.Id.ToString(),
					text = x.Name
				}), JsonRequestBehavior.AllowGet);
			}
			var theme = context.Theme.Find(guid);
			if (theme != null)
			{
				//уровень тем - возвращаем список лекций и тестов
                return Json(theme.ThemeContents.ToList().OrderBy(x => x.OrderNumber).Select(x => new {
					iconCls = (x.Type == "lecture") ? "lecture" : "test",
					id = x.Id.ToString(),
					text = x.Name
				}), JsonRequestBehavior.AllowGet);
			}
            var tc = context.ThemeContent.Find(guid);
            if (tc != null)
            {
                if (tc.Type == "lecture")
                {
                    //уровень лекций - возвращаем список параграфов
                    return Json(tc.Paragraphs.ToList().OrderBy(x => x.OrderNumber).Select(x => new {
                        iconCls = "paragraph",
                        id = x.Id.ToString(),
                        text = x.Header,
                        leaf = true
                    }), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //уровень тестов - возвращаем список вопросов
                    return Json(tc.Questions.ToList().OrderBy(x => x.OrderNumber).Select(x => new {
                        iconCls = "question",
                        id = x.Id.ToString(),
                        text = "Вопрос №" + x.OrderNumber,
                        leaf = true
                    }), JsonRequestBehavior.AllowGet);
                }
            }
			return new EmptyResult();
		}

        #region Read-Save
        /*метод, который возвращает информацию о курсе, теме или содержимом темы в зависимости от глубины дерева,
         переданной параметром. Информация однотипная, поэтому я и объединил три разных запроса-ответа в один метод*/
        public ActionResult ReadCTTC(Guid id, int depth)
        {
            if (depth == 1) 
            {
                var c = context.Course.Find(id);
                return Json(new { success = true, data = new {
                    id = c.Id, name = c.Name, type = "course", ordernumber = ""
                } }, JsonRequestBehavior.AllowGet);
            }
            if (depth == 2)
            {
                var t = context.Theme.Find(id);
                return Json(new { success = true, data = new {
                    id = t.Id, name = t.Name, type = "theme", ordernumber = t.OrderNumber
                } }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var tc = context.ThemeContent.Find(id);
                return Json(new { success = true, data = new {
                    id = tc.Id, name = tc.Name, type = tc.Type, ordernumber = tc.OrderNumber
                } }, JsonRequestBehavior.AllowGet);
            }
        }

        /*метод, сохраняющий изменения в имени указанного курса / темы / содержимого. Самый простенький*/
        public ActionResult SaveCTTC(Guid id, string type, string name)
        {
            if (type == "course") { var c = context.Course.Find(id); c.Name = name; }
            else if (type == "theme") { var t = context.Theme.Find(id); t.Name = name; }
            else { var tc = context.ThemeContent.Find(id); tc.Name = name; }
            context.SaveChanges();
            return Json(new { success = true });
        }

        /*Чтение параграфа. Немного посложнее, т.к. количество картинок заранее неизвестно, и размер JSON-ответа
        получается динамическим. Через словари, возможно, было бы логичнее, но пусть остается так, для разнообразия*/
        public ActionResult ReadParagraph(Guid id)
        {
            var p = context.Paragraph.Find(id);
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string s = "{ success : true, data : {";
            s += " \"id\" : \"" + p.Id + "\",";
            s += " \"ordernumber\" : \"" + p.OrderNumber + "\",";
            s += " \"header\" : \"" + p.Header + "\",";
            s += " \"text\" : \"" + p.Text + "\",";
            s += " \"piccount\" : " + p.Pictures.Count;
            foreach (var x in p.Pictures.OrderBy(x => x.OrderNumber)) 
            {
                s += ", \"pic" + x.OrderNumber + "_path\" : \"" + x.Path + "\"";
            }
            for (var i = p.Pictures.Count + 1; i <= 20; i++)
            {
                s += ", \"pic" + i + "_path\" : \"\"";
            }
            s += "} }";
            object obj = sr.Deserialize<Object>(s);
            return Json(sr.Deserialize<Object>(s), JsonRequestBehavior.AllowGet);
        }
   
        [ValidateInput(false)]
        public string SaveParagraph(Guid id, string header, string text, int piccount)
        {
            var p = context.Paragraph.Find(id);
            p.Header = header;
            p.Text = text;

            string path = "";
            path += "/Course_" + p.ThemeContent.Theme.Course_Id.ToString();
            path += "/Theme_" + p.ThemeContent.Theme_Id.ToString();
            path += "/Lecture_" + p.ThemeContent_Id.ToString();
            path += "/Paragraph_" + p.Id.ToString();
            string physpath = Server.MapPath("~/Content/pics_base") + path;
            string virtpath = HttpContext.Request.Url.ToString().Replace("/Struct/SaveParagraph", "") + "/Content/pics_base" + path;
            if (!Directory.Exists(physpath)) Directory.CreateDirectory(physpath);     

            if (p.Pictures.Count > piccount)
            {
                while (p.Pictures.Count > piccount) context_obj.DeleteObject(p.Pictures.OrderBy(x => x.OrderNumber).Last());
            }
            else if (p.Pictures.Count < piccount)
            {
                for (var i = p.Pictures.Count + 1; i <= piccount; i++)
                    p.Pictures.Add(new Picture { OrderNumber = i, Path = null });
            }
            context_obj.SaveChanges();

            foreach (var pic in p.Pictures.OrderBy(x => x.OrderNumber))
            {
                HttpPostedFileBase file = HttpContext.Request.Files["pic" + pic.OrderNumber + "_file"];
                if (file.ContentLength != 0)
                {
                    if (System.IO.File.Exists(pic.Path)) System.IO.File.Delete(pic.Path);
                    file.SaveAs(physpath + "/" + pic.OrderNumber + "_" + file.FileName);
                    pic.Path = virtpath + "/" + pic.OrderNumber + "_" + file.FileName;                    
                }                
            }

            context.SaveChanges();
            return "{\"success\":true}";
        }

        public ActionResult ReadQuestion(string id_s)
        {
            //HttpContext.Request.UserLanguages.Count();
            var c = context.Question.Find(Guid.Parse(id_s));
            return Json(new { success = true, data = new {
                id = c.Id,
                ordernumber = c.OrderNumber,
                text = c.Text,
                picq_path = (c.PicQ != null) ? c.PicQ : "",
                pica_path = (c.PicA != null) ? c.PicA : "",                
                rb = (c.IfPictured) ? "by_pic" : "by_txt",
                anscount1 = c.AnswerVariants.Count,
                anscount2 = c.AnswerVariants.Count,                
                q1_text = (!c.IfPictured) ? c.AnswerVariants.Single(x => x.OrderNumber == 1).Text : "",
                q2_text = (!c.IfPictured) ? c.AnswerVariants.Single(x => x.OrderNumber == 2).Text : "",
                q3_text = (!c.IfPictured && (c.AnswerVariants.Count >= 3)) ? c.AnswerVariants.Single(x => x.OrderNumber == 3).Text : "",
                q4_text = (!c.IfPictured && (c.AnswerVariants.Count >= 4)) ? c.AnswerVariants.Single(x => x.OrderNumber == 4).Text : "",
                q5_text = (!c.IfPictured && (c.AnswerVariants.Count >= 5)) ? c.AnswerVariants.Single(x => x.OrderNumber == 5).Text : "",
                q1_stat = (!c.IfPictured) ? c.AnswerVariants.Single(x => x.OrderNumber == 1).IfCorrect : false,
                q2_stat = (!c.IfPictured) ? c.AnswerVariants.Single(x => x.OrderNumber == 2).IfCorrect : false,
                q3_stat = (!c.IfPictured && (c.AnswerVariants.Count >= 3)) ? c.AnswerVariants.Single(x => x.OrderNumber == 3).IfCorrect : false,
                q4_stat = (!c.IfPictured && (c.AnswerVariants.Count >= 4)) ? c.AnswerVariants.Single(x => x.OrderNumber == 4).IfCorrect : false,
                q5_stat = (!c.IfPictured && (c.AnswerVariants.Count >= 5)) ? c.AnswerVariants.Single(x => x.OrderNumber == 5).IfCorrect : false,
                avp1 = (c.IfPictured) ? c.AnswerVariants.Single(x => x.OrderNumber == 1).IfCorrect : false,
                avp2 = (c.IfPictured) ? c.AnswerVariants.Single(x => x.OrderNumber == 2).IfCorrect : false,
                avp3 = (c.IfPictured && (c.AnswerVariants.Count >= 3)) ? c.AnswerVariants.Single(x => x.OrderNumber == 3).IfCorrect : false,
                avp4 = (c.IfPictured && (c.AnswerVariants.Count >= 4)) ? c.AnswerVariants.Single(x => x.OrderNumber == 4).IfCorrect : false,
                avp5 = (c.IfPictured && (c.AnswerVariants.Count >= 5)) ? c.AnswerVariants.Single(x => x.OrderNumber == 5).IfCorrect : false
            } }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public string SaveQuestion(Guid id, string text, string rb, int? anscount1, int? anscount2,
                                         HttpPostedFileWrapper picq_file, HttpPostedFileWrapper pica_file,
                                         string q1_text, string q2_text, string q3_text, string q4_text, string q5_text,
                                         string q1_stat, string q2_stat, string q3_stat, string q4_stat, string q5_stat,
                                         string avp1, string avp2, string avp3, string avp4, string avp5)
        {
            var q = context.Question.Find(id);
            q.Text = text;

            if (picq_file != null)
            {
                string path = "";
                path += "/Course_" + q.ThemeContent.Theme.Course_Id.ToString();
                path += "/Theme_" + q.ThemeContent.Theme_Id.ToString();
                path += "/Test_" + q.ThemeContent_Id.ToString();
                string physpath = Server.MapPath("~/Content/pics_base") + path;
                string virtpath = HttpContext.Request.Url.ToString().Replace("/Struct/SaveQuestion","") + "/Content/pics_base" + path;
                if (!Directory.Exists(physpath)) Directory.CreateDirectory(physpath);
                if (System.IO.File.Exists(q.PicQ)) System.IO.File.Delete(q.PicQ); //если есть старый, то удаляем его
                physpath += "/" + q.OrderNumber + "q_" + picq_file.FileName;
                virtpath += "/" + q.OrderNumber + "q_" + picq_file.FileName;
                picq_file.SaveAs(physpath);
                q.PicQ = virtpath;
            }

            if (rb == "by_txt")
            {
                q.IfPictured = false; q.PicA = null;
                while (q.AnswerVariants.Count() > 0) context_obj.DeleteObject(q.AnswerVariants.First());
                q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 1, Text = q1_text, Question = q, IfCorrect = (q1_stat != null) ? true : false});
                q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 2, Text = q2_text, Question = q, IfCorrect = (q2_stat != null) ? true : false });
                if (anscount1 >= 3) q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 3, Question = q, Text = q3_text, IfCorrect = (q3_stat != null) ? true : false });
                if (anscount1 >= 4) q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 4, Question = q, Text = q4_text, IfCorrect = (q4_stat != null) ? true : false });
                if (anscount1 == 5) q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 5, Question = q, Text = q5_text, IfCorrect = (q5_stat != null) ? true : false });
            }
            else
            {
                q.IfPictured = true;
                while (q.AnswerVariants.Count() > 0) context_obj.DeleteObject(q.AnswerVariants.First());
                q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 1, Text = null, IfCorrect = (avp1 != null) ? true : false });
                q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 2, Text = null, IfCorrect = (avp2 != null) ? true : false });
                if (anscount2 >= 3) q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 3, Text = null, IfCorrect = (avp3 != null) ? true : false });
                if (anscount2 >= 4) q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 4, Text = null, IfCorrect = (avp4 != null) ? true : false });
                if (anscount2 == 5) q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 5, Text = null, IfCorrect = (avp5 != null) ? true : false });
                if (pica_file != null)
                {
                    string path = "";
                    path += "/Course_" + q.ThemeContent.Theme.Course_Id.ToString();
                    path += "/Theme_" + q.ThemeContent.Theme_Id.ToString();
                    path += "/Test_" + q.ThemeContent_Id.ToString();
                    string physpath = Server.MapPath("~/Content/pics_base") + path;
                    string virtpath = HttpContext.Request.Url.ToString().Replace("/Struct/SaveQuestion", "") + "/Content/pics_base" + path;
                    if (!Directory.Exists(physpath)) Directory.CreateDirectory(physpath);
                    if (System.IO.File.Exists(q.PicA)) System.IO.File.Delete(q.PicA); //если есть старый, то удаляем его
                    physpath += "/" + q.OrderNumber + "a_" + pica_file.FileName;
                    virtpath += "/" + q.OrderNumber + "a_" + pica_file.FileName;
                    pica_file.SaveAs(physpath);
                    q.PicA = virtpath;
                }
            }

            context.SaveChanges(); context_obj.SaveChanges();
            return "{\"success\":true}";
        }
        #endregion

        #region Add-Remove
        /*Дальше идет ряд однотипных методов по добавлению / удалению сущностей. Есть одна тонкость, связанная с порядковыми
        номерами: при добавлении новой сущности нужно найти ее соседа с самым большим номером и сделать плюс один, при
        удалении - сделать минус один всем соседям, номера которых больше номера удаленной сущности*/
        public Guid AddCourse()
        {
            var c = new Course { Name = "Новый курс" };
            context.Course.Add(c);
            context.SaveChanges();
            return c.Id;
        }

        public string RemoveCourse(Guid id)
        {
            Course c = context.Course.Find(id);
            string path = Server.MapPath("~/Content/pics_base") + "/Course_" + c.Id.ToString();
            if (Directory.Exists(path)) Directory.Delete(path, true);
            context_obj.DeleteObject(c);    
            context_obj.SaveChanges();
            return "OK";
        }

        public Guid AddTheme(Guid parent_id)
        {
            var c = context.Course.Find(parent_id);
            int ordnmb;
            if (c.Themes.Count == 0) ordnmb = 1;
            else ordnmb = c.Themes.OrderBy(x => x.OrderNumber).Last().OrderNumber + 1;
            var t = new Theme
            {
                OrderNumber = ordnmb, Name = "Новая тема"
            };
            c.Themes.Add(t);
            context.SaveChanges();
            return t.Id;
        }

        public string RemoveTheme(Guid id, Guid parent_id)
        {
            Theme t = context.Theme.Find(id);
            string path = Server.MapPath("~/Content/pics_base") + "/Course_" + t.Course_Id.ToString() + "/Theme_" + t.Id.ToString();
            if (Directory.Exists(path)) Directory.Delete(path, true);
            context_obj.DeleteObject(t);
            context_obj.SaveChanges();

            int i = 1;
            foreach (var x in context.Course.Find(parent_id).Themes.OrderBy(x => x.OrderNumber))
            {
                x.OrderNumber = i;
                i++;
            }
            context.SaveChanges();
            return "OK";
        }

        public Guid AddLecture(Guid parent_id)
        {
            var t = context.Theme.Find(parent_id);
            int num;
            if (t.ThemeContents.Count == 0) num = 1;
            else num = t.ThemeContents.OrderBy(x => x.OrderNumber).Last().OrderNumber + 1;
            var tc = new ThemeContent { OrderNumber = num, Name = "Новая лекция", Type = "lecture" };
            t.ThemeContents.Add(tc);
            context.SaveChanges();
            return tc.Id;            
        }

        public Guid AddTest(Guid parent_id)
        {
            var t = context.Theme.Find(parent_id);
            int num;
            if (t.ThemeContents.Count == 0) num = 1;
            else num = t.ThemeContents.OrderBy(x => x.OrderNumber).Last().OrderNumber + 1;
            var tc = new ThemeContent { OrderNumber = num, Name = "Новый тест", Type = "test" };
            t.ThemeContents.Add(tc);
            context.SaveChanges();
            return tc.Id;
        }

        public string RemoveContent(Guid id, Guid parent_id)
        {
            ThemeContent tc = context.ThemeContent.Find(id);
            
            string path = Server.MapPath("~/Content/pics_base");
            path += "/Course_" + tc.Theme.Course_Id.ToString();
            path += "/Theme_" + tc.Theme_Id.ToString();
            if (tc.Type == "lecture") path += "/Lecture_"; else path += "/Test_";
            path += tc.Id.ToString();             
            if (Directory.Exists(path)) Directory.Delete(path, true);

            context_obj.DeleteObject(tc);
            context_obj.SaveChanges();

            int i = 1;
            foreach (var x in context.Theme.Find(parent_id).ThemeContents.OrderBy(x => x.OrderNumber))
            {
                x.OrderNumber = i;
                i++;
            }
            context.SaveChanges();
            return "OK";
        }

        public Guid AddParagraph(Guid parent_id)
        {
            var l = context.ThemeContent.Find(parent_id);
            int num;
            if (l.Paragraphs.Count == 0) num = 1;
            else num = l.Paragraphs.OrderBy(x => x.OrderNumber).Last().OrderNumber + 1;
            var p = new Paragraph { OrderNumber = num, Header = "Новый параграф", Text = "Текст параграфа" };
            l.Paragraphs.Add(p);
            context.SaveChanges();
            return p.Id;
        }

        public string RemoveParagraph(Guid id, Guid parent_id)
        {
            Paragraph p = context.Paragraph.Find(id);

            string path = Server.MapPath("~/Content/pics_base");
            path += "/Course_" + p.ThemeContent.Theme.Course_Id.ToString();
            path += "/Theme_" + p.ThemeContent.Theme_Id.ToString();
            path += "/Lecture_" + p.ThemeContent_Id.ToString();
            path += "/Paragraph_" + p.Id.ToString();
            if (Directory.Exists(path)) Directory.Delete(path, true);

            context_obj.DeleteObject(p);
            context_obj.SaveChanges();

            int i = 1;
            foreach (var x in context.ThemeContent.Find(parent_id).Paragraphs.OrderBy(x => x.OrderNumber))
            {
                x.OrderNumber = i;
                i++;
            }
            context.SaveChanges();
            return "OK";
        }

        public Guid AddQuestion(Guid parent_id)
        {
            var t = context.ThemeContent.Find(parent_id);
            int num;
            if (t.Questions.Count == 0) num = 1;
            else num = t.Questions.OrderBy(x => x.OrderNumber).Last().OrderNumber + 1;
            var q = new Question
            {
                OrderNumber = num,
                Text = "Текст вопроса",
                PicQ = null,
                IfPictured = false,
                PicA = null
            };
            q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 1, Text = "Вариант ответа №1", IfCorrect = false });
            q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 2, Text = "Вариант ответа №2", IfCorrect = false });
            q.AnswerVariants.Add(new AnswerVariant { OrderNumber = 3, Text = "Вариант ответа №3", IfCorrect = false });
            t.Questions.Add(q);
            context.SaveChanges();
            return q.Id;
        }

        public string RemoveQuestion(Guid id, Guid parent_id)
        {
            Question q = context.Question.Find(id);
            context_obj.DeleteObject(q);
            context_obj.SaveChanges();

            int i = 1;
            foreach (var x in context.ThemeContent.Find(parent_id).Paragraphs.OrderBy(x => x.OrderNumber))
            {
                x.OrderNumber = i;
                i++;
            }
            context.SaveChanges();
            return "OK";
        }
        #endregion

        #region Move
        /*А это два метода, которые меняют местами порядковые номера этой и предыдущей (MoveUp) либо этой и следующей
        (MoveDown) сущностей. В редакторе курсов это пока что единственное средство, с помощью которого можно регулировать
        порядок следования тем и всего прочего, но его должно быть вполне достаточно*/
        public string MoveUp(Guid id, Guid parent_id, int depth, string type)
        {
            if (depth == 2)
            {
                var this_one = context.Theme.Find(id);
                int num = this_one.OrderNumber;
                if (num > 1)
                {
                    var parent = context.Course.Find(parent_id);
                    var prev_one = parent.Themes.Where(x => x.OrderNumber == num - 1).First();
                    this_one.OrderNumber = num - 1;
                    prev_one.OrderNumber = num;                    
                }
            }
            else if (depth == 3)
            {
                var this_one = context.ThemeContent.Find(id);
                int num = this_one.OrderNumber;
                if (num > 1)
                {
                    var parent = context.Theme.Find(parent_id);
                    var prev_one = parent.ThemeContents.Where(x => x.OrderNumber == num - 1).First();
                    this_one.OrderNumber = num - 1;
                    prev_one.OrderNumber = num;                    
                }
            }
            else if (type == "paragraph")
            {
                var this_one = context.Paragraph.Find(id);
                int num = this_one.OrderNumber;
                if (num > 1)
                {
                    var parent = context.ThemeContent.Find(parent_id);
                    var prev_one = parent.Paragraphs.Where(x => x.OrderNumber == num - 1).First();
                    this_one.OrderNumber = num - 1;
                    prev_one.OrderNumber = num;                    
                }
            }
            else
            {
                var this_one = context.Question.Find(id);
                int num = this_one.OrderNumber;
                if (num > 1)
                {
                    var parent = context.ThemeContent.Find(parent_id);
                    var prev_one = parent.Questions.Where(x => x.OrderNumber == num - 1).First();
                    this_one.OrderNumber = num - 1;
                    prev_one.OrderNumber = num;                    
                }
            }
            context.SaveChanges();
            return "OK";
        }

        public string MoveDown(Guid id, Guid parent_id, int depth, string type)
        {
            if (depth == 2)
            {
                var this_one = context.Theme.Find(id);
                var parent = context.Course.Find(parent_id);
                int num = this_one.OrderNumber;
                int num_max = parent.Themes.OrderBy(x => x.OrderNumber).Last().OrderNumber;
                if (num < num_max)
                {
                    var next_one = parent.Themes.First(x => x.OrderNumber == num + 1);
                    this_one.OrderNumber = num + 1;
                    next_one.OrderNumber = num;
                }
            }
            else if (depth == 3)
            {
                var this_one = context.ThemeContent.Find(id);
                var parent = context.Theme.Find(parent_id);
                int num = this_one.OrderNumber;
                int num_max = parent.ThemeContents.OrderBy(x => x.OrderNumber).Last().OrderNumber;
                if (num < num_max)
                {
                    var next_one = parent.ThemeContents.First(x => x.OrderNumber == num + 1);
                    this_one.OrderNumber = num + 1;
                    next_one.OrderNumber = num;                    
                }
            }
            else if (type == "paragraph")
            {
                var this_one = context.Paragraph.Find(id);
                var parent = context.ThemeContent.Find(parent_id);
                int num = this_one.OrderNumber;
                int num_max = parent.Paragraphs.OrderBy(x => x.OrderNumber).Last().OrderNumber;
                if (num < num_max)
                {
                    var next_one = parent.Paragraphs.First(x => x.OrderNumber == num + 1);
                    this_one.OrderNumber = num + 1;
                    next_one.OrderNumber = num;                    
                }
            }
            else
            {
                var this_one = context.Question.Find(id);
                var parent = context.ThemeContent.Find(parent_id);
                int num = this_one.OrderNumber;
                int num_max = parent.Questions.OrderBy(x => x.OrderNumber).Last().OrderNumber;
                if (num < num_max)
                {
                    var next_one = parent.Questions.First(x => x.OrderNumber == num + 1);
                    this_one.OrderNumber = num + 1;
                    next_one.OrderNumber = num;                    
                }
            }
            context.SaveChanges();
            return "OK";
        }
        #endregion

        /*пример работы со словарями
        public ActionResult ReadTest(Guid id)
        {
            var test = context.Test.Single(x => x.Id == id);
            var dict = new Dictionary<string, string>();
            dict["id"] = test.Id.ToString();
            dict["name"] = test.Name;
            dict["trnd"] = test.RandomizeQuestions.ToString().ToLowerInvariant();
            var closedQuestions = test.Questions.OfType<ClosedQuestion>();
            for (int i = 1; i <= closedQuestions.Count(); i++)
            {
                var q = closedQuestions.ElementAt(i - 1);
                dict["type" + i] = "text";
                dict["qid" + i] = q.Id.ToString();
                dict["question" + i] = q.Text;
                dict["qrnd" + i] = q.RandomizeAnswers.ToString().ToLowerInvariant();
                for (int j = 1; j <= q.Answers.Count; j++)
                {
                    var a = q.Answers.ElementAt(j - 1);
                    dict["aid" + i + j] = a.Id.ToString();
                    dict["answer" + i + j] = a.Text;
                    dict["correct" + i + j] = (a.Weight > 0).ToString().ToLowerInvariant();
                }
            }
            return Json(new Dictionary<string, object>() { 
				{"data", dict },
				{"success", "true"}
			}, JsonRequestBehavior.AllowGet);
        }*/
    }
}

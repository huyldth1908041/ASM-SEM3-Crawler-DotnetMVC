﻿using Newtonsoft.Json;
using ReadNewsWebClient.API;
using ReadNewsWebClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebClient.Controllers
{
    public class ArticleController : Controller
    {
        private List<Article> _articles;
        private List<Category> _categories;
        public ArticleController()
        {
            //hard code for category
            _categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Thời sự"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Giải trí"
                }
            };
            //hard code for articles
            //_articles = new List<Article>()
            //{
            //    new Article()
            //    {
            //        Id = 0,
            //        Content = "<p class='article-paragraph'>Ngày 2/3, UBND tỉnh Quảng Ninh cho phép thí điểm mở lại một số hoạt động kinh tế xã hội trong trạng thái bình thường mới. Tại cảng tàu du lịch, điểm tham quan trên vịnh Hạ Long, chủ tàu phải đảm bảo giữ khoảng cách tối thiểu giữa các hành khách và có các biện pháp phòng, chống dịch; bố trí tối đa không quá 50% số ghế trên tàu. Tàu lưu trú được phép chở số khách theo đăng ký phù hợp với thực tiễn của tàu, không tập trung đông người tại các khu vực công cộng trên tàu, bố trí lệch giờ ăn, giờ sinh hoạt chung...</p><figure class='img-container'><img src = 'https://i1-vnexpress.vnecdn.net/2021/03/02/anh-1-JPG-2160-1614686578.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=mE8BKyFL4xF8Yz9R27lJ7A' class='article-img'><figcaption><p class='img-caption'>Khu di tích danh thắng Yên Tử, Quảng Ninh.Ảnh: Minh Cương</figcaption></figure><p class='article-paragraph'>Tỉnh Quảng Ninh cho phép tổ chức hội nghị, hội thảo, hoạt động, dịch vụ văn hóa, thể thao, phòng tập gym, fitness, yoga, câu lạc bộ bia; đám cưới, đám hỏi, nhưng phải đảm bảo giãn cách.Riêng thị xã Đông Triều chưa được tổ chức lại các hoạt động du lịch, dịch vụ văn hóa, thể thao, hội nghị, hội thảo, cưới hỏi... cho đến khi có thông báo mới.</p><p class='article-paragraph'>Với người từ tỉnh ngoài vào Quảng Ninh, tỉnh bố trí cơ sở cách ly tập trung theo hình thức tự trả phí đối với tất cả người đến từ vùng có dịch của các địa phương giáp ranh với Quảng Ninh và ổ dịch tại các tỉnh, thành phố khác trong nước.</p><p class='article-paragraph'>Người đến Quảng Ninh từ các vùng không có dịch sẽ khai báo y tế khi qua chốt kiểm soát.Tại 12 chốt kiểm soát ở cửa ngõ, nhà chức trách không kiểm soát người ra khỏi tỉnh. Tất cả chốt kiểm soát nội tỉnh dừng hoạt động.</p><p class='article-paragraph'>Từ ngày 28/1 đến sáng 2/3, Quảng Ninh ghi nhận 61 ca Covid-19. Tỉnh từng phong tỏa 14 xã, phường của thị xã Đông Triều và đảo Cái Bầu ở huyện Vân Đồn.Ngoài ra, TP Hạ Long, TP Cẩm Phả, TP Uông Bí cũng có một số khu vực bị phong tỏa cục bộ. Đến nay, tất cả xã, phường đã được dỡ cách ly.</p> <p class='article-paragraph'><strong>Minh Cương</strong><a class='email' href='javascript:;' id='send_mail_author' data-article-id='4242560'><i class='ic ic-email'></i></a></p>",
            //        Description = "Từ 0h ngày 2/3 đến 15/3, các cơ sở dịch vụ, du lịch, tín ngưỡng tôn giáo ở Quảng Ninh được hoạt động trở lại, nhưng chỉ đón khách nội tỉnh.",
            //        ImgUrls ="https://i1-vnexpress.vnecdn.net/2021/03/02/anh-1-JPG-2160-1614686578.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=mE8BKyFL4xF8Yz9R27lJ7A",
            //        Link = "https://vnexpress.net/quang-ninh-noi-long-mot-so-dich-vu-4242560.html",
            //        Source = "https://vnexpress.net",
            //        Status = Article.ArticleStatus.ACTIVE,
            //        CategoryId = 1,
            //        CreatedAt = DateTime.Now,
            //        Title = "Từ 0h ngày 2/3 đến 15/3, các cơ sở dịch vụ, du lịch, tín ngưỡng tôn giáo ở Quảng Ninh được hoạt động trở lại, nhưng chỉ đón khách nội tỉnh."
            //    },
            //    new Article()
            //    {
            //        Id = 1,
            //        Content = "<p class='article-paragraph'>UBND tỉnh Hải Dương ngày 2/3 ra quyết định gỡ lệnh phong tỏa với TP Chí Linh và huyện Cẩm Giàng, chuyển toàn tỉnh sang trạng thái mới phòng chống Covid-19 sau ngày kết thúc cách ly xã hội, từ 0h ngày 3/3.</p><p class='article-paragraph'>Chính quyền phân chia 12 đơn vị hành chính cấp huyện ra làm hai nhóm nhằm kiểm soát tình hình dịch bệnh hiệu quả hơn. Những nơi đang bị phong tỏa tiếp tục thực hiện cho đến khi đủ thời gian quy định.</p><figure class='img-container'><img src = 'https://i1-vnexpress.vnecdn.net/2021/03/02/chilinh1-3472-1614678743.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=GcjNgIv8MzQ3ZH1AXTmbvQ' class='article-img'><figcaption><p class='img-caption'>Ngã tư quốc lộ 18 chạy qua TP Chí Linh vắng bóng xe cộ trong thời gian cách ly, từ ngày mai các phương tiện vận tải sẽ được phép lưu thông.Ảnh: Hà Bi</figcaption></figure><p class='article-paragraph'><span style = 'color:#000000;' >< strong > Bốn huyện thị gồm Cẩm Giàng, Kim Thành, thị xã Kinh Môn và TP Hải Dương thực hiện Chỉ thị 15</strong></span>, tiếp tục giãn cách xã hội cho tới ngày 17/3. Các sự kiện, hội họp tại bốn huyện thị này không được quá 20 người một phòng, không tụ tập từ 10 người trở lên ngoài công sở, trường học, bệnh viện và giữ khoảng cách tối thiểu 2 m tại các địa điểm công cộng.Tỉnh khuyến cáo người dân không ra khỏi nhà nếu không cần thiết, thực hiện 5K của Bộ Y tế.</p><p class='article-paragraph'>Tỉnh tiếp tục dừng các nghi lễ tôn giáo, tín ngưỡng, thờ tự từ 20 người trở lên; dừng tất cả hoạt động văn hóa, thể thao, giải trí tại các địa điểm công cộng; các hoạt động kinh doanh dịch vụ không thiết yếu tại khu di tích, khu vui chơi, giải trí, khu tập luyện thể thao, cơ sở làm đẹp, gội đầu, karaoke, massage, quán bar, vũ trường; quán ăn không phục vụ tại chỗ.Chủ tịch huyện có quyền quyết định các cơ sở kinh doanh, dịch vụ khác cần đóng cửa.</p><p class='article-paragraph'>Học sinh các cấp ở bốn huyện thị này tiếp tục nghỉ học cho đến khi dịch được kiểm soát hoàn toàn, trước mắt là đến ngày 17/3. Các trường tổ chức dạy học trực tuyến cho học sinh từ cấp tiểu học trở lên trong thời gian trên.</p><figure class='img-container'><img src = 'https://i1-vnexpress.vnecdn.net/2021/03/02/chilinh2-7740-1614678743.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=KdBZBKLNWzcA0sraSBN7DQ' class='article-img'><figcaption><p class='img-caption'>Hải Dương cho phép các hoạt động công cộng nhưng không tập trung quá 30 người, khuyến khích người dân tiếp tục ở nhà nếu không có việc cấp bách phải ra ngoài.Ảnh: Hà Bi</figcaption></figure><p class='article-paragraph'><span style = 'color:#000000;' >< strong > Tám huyện, thành phố còn lại gồm Bình Giang, Gia Lộc, Nam Sách, Ninh Giang, Thanh Hà, Thanh Miện, Tứ Kỳ và TP Chí Linh thực hiện Chỉ thị 19</strong></span>, có thể nới lỏng biện pháp chống dịch, khôi phục một số hoạt động kinh tế cho tới khi dập dịch hoàn toàn.</p><p class='article-paragraph'>Chính quyền yêu cầu không tập trung quá 30 người tại nơi công cộng, ngoài phạm vi công sở, trường học, bệnh viện; giữ khoảng cách tối thiểu 2 m khi tiếp xúc.Người dân thực hiện khuyến cáo 5K của Bộ Y tế.</p><p class='article-paragraph'>Các lễ hội, nghi lễ tôn giáo, sự kiện tập trung quá 30 người tại nơi công cộng, sân vận động và sự kiện lớn chưa cần thiết tiếp tục tạm dừng.Ngoài ra, chính quyền tạm đình chỉ hoạt động các cơ sở kinh doanh dịch vụ không thiết yếu: khu vui chơi, giải trí, cơ sở làm đẹp, karaoke, massage, quán bar, vũ trường, quán game; cơ sở kinh doanh thương mại dịch vụ như bán buôn, bán lẻ, xổ số kiến thiết, khách sạn, cơ sở lưu trú, nhà hàng, quán ăn, khu tập luyện thể thao, khu di tích, danh lam thắng cảnh.</p><p class='article-paragraph'>Học sinh tại 8 huyện thị này tiếp tục nghỉ học đến ngày 17/3. Riêng khối 12 các trường trung học phổ thông, trung tâm giáo dục nghề nghiệp, giáo dục thường xuyên có thể trở lại trường nếu đảm bảo các biện pháp phòng dịch.</p><figure class='img-container'><img src = 'https://i1-vnexpress.vnecdn.net/2021/03/02/vantai1-4441-1614678743.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=mhPBaXLi4H_0ZnmEM_HhFg' class='article-img'><figcaption><p class='img-caption'>Container phải quay đầu trước chốt kiểm soát khi Hải Dương cách ly xã hội 15 ngày.Ảnh: Giang Huy</figcaption></figure><p class='article-paragraph'><span style = 'color:#000000;' >< strong > Hải Dương<a href='https://vnexpress.net/tai-xe-di-the-nao-khi-hai-duong-cach-ly-xa-hoi-4235886.html' rel='dofollow'> khôi phục lưu thông</a> của phương tiện vận tải trên quốc lộ qua địa bàn, vốn bị dừng từ ngày 16/2; </strong></span>cho phép vận chuyển khách bằng taxi, xe buýt, xe tuyến cố định, xe hợp đồng.Nhà ga, bến xe khách, bến khách ngang sông, bến phà được hoạt động, nhưng phải đảm bảo phòng dịch.</p><p class='article-paragraph'>Riêng phương tiện của bốn huyện thị Cẩm Giàng, Kinh Môn, Kim Thành, TP Hải Dương chỉ được hoạt động trong nội tỉnh, hạn chế ra tỉnh ngoài.Vận tải ở tám địa phương còn lại được phép hoạt động nội tỉnh và liên tỉnh. Sở Giao thông Vận tải Hải Dương sẽ căn cứ tình hình thực tế, thông báo thời điểm và điều kiện hoạt động trở lại cụ thể của từng lĩnh vực.</p><p class='article-paragraph'>Hải Dương trở thành ổ dịch lớn nhất nước, ghi nhận 684 ca nhiễm Covid-19 tính từ 28/1. Sáng nay, Hải Dương thêm 11 ca mắc mới, trong đó 10 ca thuộc xã Kim Đính(huyện Kim Thành), nơi vừa phong tỏa toàn xã với 7.400 cư dân.Từ 0h ngày 3/3, tỉnh này sẽ dừng cách ly xã hội theo Chỉ thị 16 của Thủ tướng.</p><p style = 'text-align:right;' class='article-paragraph'><strong>Hoàng Phương</strong></p><div class='component corona_virus_new' contenteditable='false' data-component='true' data-component-detail='true' data-component-type='virus_covid_19' data-component-widget-province='false' data-component-widget-type='widget_023' data-widget='obj'></div>",
            //        Title = "Ôtô được lưu thông trên quốc lộ qua tỉnh Hải Dương, xe khách, xe buýt, bến phà được chở khách sau 15 ngày ngừng hoạt động.",
            //        ImgUrls = "https://i1-vnexpress.vnecdn.net/2021/03/02/chilinh1-3472-1614678743.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=GcjNgIv8MzQ3ZH1AXTmbvQ,https://i1-vnexpress.vnecdn.net/2021/03/02/chilinh2-7740-1614678743.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=KdBZBKLNWzcA0sraSBN7DQ,https://i1-vnexpress.vnecdn.net/2021/03/02/vantai1-4441-1614678743.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=mhPBaXLi4H_0ZnmEM_HhFg",
            //        Link = "https://vnexpress.net/quang-ninh-noi-long-mot-so-dich-vu-4242560.html",
            //        Description = "Ôtô được lưu thông trên quốc lộ qua tỉnh Hải Dương, xe khách, xe buýt, bến phà được chở khách sau 15 ngày ngừng hoạt động.",
            //        CategoryId = 1,
            //        Source = "https://vnexpress.net",
            //        CreatedAt = DateTime.Now,
            //        Status = Article.ArticleStatus.ACTIVE,
                    
                    
            //    },
            //    new Article()
            //    {
            //        Id = 2,
            //        Content = "<p class='article-paragraph'>Từ sau Tết Nguyên đán đến nay, cánh đồng rau ở nhiều xã của huyện Quỳnh Lưu vào vụ thu hoạch, nhưng không khí ảm đạm. Đứng ở đầu ruộng, bà Hồ Thị Ngọ, xã Quỳnh Lương, cho biết gia đình có hơn 3 sào (mỗi sào 500 m2), chủ yếu trồng cải bắp, rau cúc, hành hoa, cà chua, su hào, đều được mùa.</p><p class='article-paragraph'>Tiền đầu tư và công chăm sóc hơn 4 triệu đồng/sào, nếu giá bán như các năm trước 4.000-6.000 đồng/kg cải bắp thì gia đình bà Ngọ bình quân thu được 7 triệu đồng/sào. 'Năm nay giá chỉ 300-500 đồng/kg nhưng người mua cũng lẻ tẻ. Cả một đống rau chất cao ngút mà được hơn 100.000 đồng, chưa đủ công thuê hái. Bán không hết, tôi đành chặt bỏ', bà Ngọ nói.</p><figure class='img-container'><img src='https://iv1.vnecdn.net/vnexpress/images/web/2021/03/02/nong-dan-vut-bo-rau-1614680794.jpg?w=750&h=450&q=100&dpr=1&fit=crop&s=pm7FBESxVW4eDGH65bmLRw' class='article-img'><figcaption><p class='img-caption'>Bà Hồ Thị Ngọ phá bỏ vườn rau nhà mình. Video: Nguyễn Hải.</figcaption></figure><figure class='img-container'><img src='https://i1-vnexpress.vnecdn.net/2021/03/02/rau-mat-gia-1542-1614677613.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=B93t88rL36h0dGX7H9YbqQ' class='article-img'><figcaption><p class='img-caption'>Cách ruộng nhà bà Ngọ 2 km, cánh đồng rau ở xã Quỳnh Minh vào vụ thu hoạch, nhưng nhiều hộ cũng đang nhổ bỏ, tấp đống đầy lên lề đường hoặc đem về làm thức ăn cho lợn, bò.</figcaption></figure> <p class='article-paragraph'><strong>Nguyễn Hải</strong><a class='email' href='javascript:;' id='send_mail_author' data-article-id='4242458'><i class='ic ic-email'></i></a></p>",
            //        Title = "Nhiều diện tích rau, củ ở Nghệ An vào vụ thu hoạch, song không có người mua nên nông dân đành chặt bỏ hoặc tận dụng làm thức ăn gia súc.",
            //        Link = "https://vnexpress.net/nong-dan-nghe-an-vut-bo-rau-xanh-4242458.html",
            //        Source = "https://vnexpress.net",
            //        ImgUrls = "https://iv1.vnecdn.net/vnexpress/images/web/2021/03/02/nong-dan-vut-bo-rau-1614680794.jpg?w=750&h=450&q=100&dpr=1&fit=crop&s=pm7FBESxVW4eDGH65bmLRw,https://i1-vnexpress.vnecdn.net/2021/03/02/rau-mat-gia-1542-1614677613.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=B93t88rL36h0dGX7H9YbqQ",
            //        CreatedAt = DateTime.Now,
            //        CategoryId = 1,
            //        Description = "Nhiều diện tích rau, củ ở Nghệ An vào vụ thu hoạch, song không có người mua nên nông dân đành chặt bỏ hoặc tận dụng làm thức ăn gia súc.",
            //        Status = Article.ArticleStatus.ACTIVE
            //    },
            //    new Article()
            //    {
            //        Id = 3,
            //        Content = "<p class='article-paragraph'>Thông tin được Phó chủ tịch UBND TP HCM Võ Văn Hoan nói tại cuộc họp trực tuyến của Chính phủ với các tỉnh, thành về tình hình kinh tế - xã hội tháng 2 và hai tháng đầu năm, sáng 2/3.</p><p class='article-paragraph'>Theo ông Hoan, năm nay Trung ương giao chỉ tiêu thu ngân sách cho TP HCM gần 365.000 tỷ đồng, tức là bình quân một ngày phải thu khoảng 1.500 tỷ. Trong tháng 2 và 2 tháng đầu năm, mỗi ngày thành phố thu khoảng 2.900 tỷ, gần như tăng gấp đôi thu bình quân một ngày.</p><p class='article-paragraph'>'Đến giờ này, thành phố đã thu 74.500 tỷ đồng, đạt 20,5% dự toán, tăng 10,5% so với cùng kỳ, với đà này thành phố có thể hoàn thành chỉ tiêu Trung ương giao', ông Hoan nói.</p><p class='article-paragraph'>Thu ngân sách của TP HCM năm 2019 đạt gần 410.000 tỷ đồng; năm 2020 ảnh hưởng dịch chỉ thu được hơn 371.000 tỷ đồng (bằng 91,5% dự toán). Những năm gần đây, thu ngân sách thành phố thường chiếm 25-27% tổng thu cả nước.</p><figure class='img-container'><img src = 'https://i1-vnexpress.vnecdn.net/2021/03/02/vo-van-hoan-8328-1614659461.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=K_Tn9Vr_s8ODprizqJfVyQ' class='article-img'><figcaption><p class='img-caption'>Phó chủ tịch UBND TP HCM Võ Văn Hoan tại cuộc họp sáng nay.Ảnh: Trung tâm báo chí TP HCM.</figcaption></figure><p class='article-paragraph'>Theo lãnh đạo UBND thành phố, trong tháng 2 và hai tháng đầu năm nay, các chỉ tiêu phát triển kinh tế - xã hội trên địa bàn có mức tăng trưởng khả quan hơn so với cùng kỳ.Cụ thể, tổng mức bán lẻ hàng hoá và doanh thu dịch vụ tăng 4,7%, thương mại bán lẻ hàng hoá tăng 11%, kim ngạch xuất khẩu đạt 8 tỷ USD, tăng 25%, gấp 3 lần cùng kỳ năm trước.</p><p class='article-paragraph'>Kim ngạch nhập khẩu hàng hoá 2 tháng đầu năm của thành phố đạt 10,9 tỷ USD, tăng 53,1% (cùng kỳ giảm 2,4%). Trong đó 3 mặt hàng làm tăng kim ngạch xuất khẩu là phân bón, chất dẻo nguyên liệu và linh kiện phụ tùng ôtô để phục vụ sản xuất; chỉ số sản xuất công nghiệp tăng 6%, có hơn 3.800 doanh nghiệp hoạt động trở lại(tăng 3%), 700 doanh nghiệp hoàn tất thủ tục giải thể(giảm 14,5%).</p><p class='article-paragraph'>Tuy nhiên, theo ông Hoan ngành dịch vụ của thành phố bị ảnh hưởng nặng nề do Covid-19. Trong 2 tháng đầu năm, du lịch lữ hành giảm sâu 70%, dịch vụ lưu trú giảm 14%, không có khách quốc tế nào đến Việt Nam.</p><p class='article-paragraph'>Đối với việc phòng chống dịch, Phó chủ tịch UBND thành phố cho biết TP HCM đã khoanh vùng và truy vết kịp thời, kiểm soát được chuỗi lây nhiễm ở sân bay Tân Sơn Nhất.Đến hôm nay, TP HCM đã có 20 ngày không phát sinh ca nhiễm mới trong cộng đồng.</p><p class='article-paragraph'>'Một số dịch vụ đang được hoạt động lại, học sinh trở lại trường từ ngày 1/3. Thành phố cũng yêu cầu các cơ quan đơn vị thực hiện tốt bộ tiêu chí bảo đảm an toàn phòng dịch trong hoạt động, sản xuất', ông Hoan nói.</p><p class='article-paragraph'>Thời gian tới, thành phố tiếp tục thực hiện các giải pháp để thực hiện chủ đề năm 2021 là<em> Năm xây dựng chính quyền đô thị và cải thiện môi trường đầu tư</em>; có các giải pháp hỗ trợ doanh nghiệp, người dân gặp khó khăn do dịch...Ngoài ra, TP HCM tiếp tục duy trì quan hệ hợp tác, kêu gọi đầu tư qua các giao thức trực tuyến giúp kinh tế thành phố không bị gián đoạn.</p><p class='article-paragraph'>'Đặc biệt, TP HCM tiếp tục triển khai các giải pháp quyết liệt để phòng chống Covid-19 với tinh thần 'không chủ quan nhưng không hoang mang', chuẩn bị sẵn sàng khi xảy ra tình huống xấu', ông Hoan nói.</p><p style = 'text-align:right;' class='article-paragraph'><strong>Hữu Công</strong></p>",
            //        Description = "Trong 2 tháng đầu năm, mỗi ngày làm việc thành phố thu ngân sách khoảng 2.900 tỷ đồng, gần gấp đôi con số bình quân một ngày Trung ương giao.",
            //        Title = "TP HCM thu ngân sách 2.900 tỷ đồng mỗi ngày",
            //        CategoryId = 1,
            //        Source = "https://vnexpress.net",
            //        ImgUrls = "https://i1-vnexpress.vnecdn.net/2021/03/02/vo-van-hoan-8328-1614659461.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=K_Tn9Vr_s8ODprizqJfVyQ",
            //        Link = "https://vnexpress.net/tp-hcm-thu-ngan-sach-2-900-ty-dong-moi-ngay-4242216.html",
            //        CreatedAt = DateTime.Now,
            //        Status =  Article.ArticleStatus.ACTIVE
            //    },
            //    new Article()
            //    {
            //        Id = 4,
            //        Content = "<p class='article-paragraph'>Đây là tuyến <a href='https://vnexpress.net/hon-4-800-ty-dong-lam-duong-moi-o-cua-ngo-tan-son-nhat-3975741.html' rel='dofollow'>xây mới</a> kết nối từ đường Trần Quốc Hoàn đến Cộng Hòa (quận Tân Bình), dài hơn 4 km. Dự án có chi phí xây lắp 1.735 tỷ đồng, còn lại là phần bồi thường, giải phóng mặt bằng. Trong báo cáo vừa gửi UBND TP HCM, Ban quản lý dự án đầu tư xây dựng các công trình giao thông (chủ đầu tư), cho biết dự án cần khởi công tháng 12 năm nay, hoàn thành sau 18 tháng. Đây là thời điểm <a href='https://vnexpress.net/pho-thu-tuong-doc-thuc-du-an-nha-ga-t3-tan-son-nhat-4189392.html' rel='dofollow'>nhà ga T3 </a>sân bay Tân Sơn Nhất hoàn thành, đưa vào khai thác như dự kiến.</p><figure class='img-container'><img src = 'https://i1-vnexpress.vnecdn.net/2021/03/01/lang-cha-ca-1805-1614615353.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=2tFfrLjYD2sUseqWWdzy0A' class='article-img'><figcaption><p class='img-caption'>Xe chạy trên đường Trần Quốc Hoàn, đoạn qua vòng xoay Lăng Cha Cả - cửa ngõ ra vào sân bay Tân Sơn Nhất, tháng 10/2020. Ảnh: Gia Minh.</figcaption></figure><p class='article-paragraph'>Công trình làm đường rộng từ 29-48 m cho 6 làn xe.Đồng thời xây 2 hầm chui tại giao lộ Phan Thúc Duyện - Trần Quốc Hoàn (dài 42 m, rộng 9 m, 2 làn xe) và nút giao Trường Chinh - Tân Kỳ Tân Quý(dài 65 m, rộng 22 m, 5 làn xe). Dự án còn làm một cầu vượt trước ga T3 sân bay Tân Sơn Nhất, dài 1,2 km, rộng 17 m cho 4 làn xe.Tuyến đường khi hoàn thành tạo thêm hướng tiếp cận sân bay, phá thế độc đạo của đường Trường Sơn và giảm kẹt xe cho các đường khác như Cộng Hoà, Hoàng Hoa Thám, Phan Thúc Duyện...</p><p class='article-paragraph'>Để kịp tiến độ, chủ đầu tư kiến nghị từ nay đến cuối năm cần đẩy nhanh hoàn tất nhiều đầu việc như điều chỉnh quy hoạch cục bộ tại khu vực; thẩm định, duyệt báo cáo nghiên cứu khả thi; giao đất quốc phòng; chọn nhà thầu thi công...Theo chủ đầu tư, tiến độ dự án phần lớn phụ thuộc vào việc thu hồi, bồi thường giải phóng mặt bằng.</p><p class='article-paragraph'>Tại dự án này, Bộ Quốc phòng hồi tháng 11 năm ngoái thống nhất phương án ranh xây dựng do phạm vi đi qua đất do Bộ quản lý, song đề nghị TP HCM phối hợp các bên hoàn tất thủ tục liên quan.Hiện, UBND thành phố đã chỉ đạo quận Tân Bình cùng các sở, ngành đẩy nhanh để sớm bàn giao đất cho dự án.</p><figure class='img-container'><img src = 'https://i1-vnexpress.vnecdn.net/2021/03/01/duong-truong-son-7349-1614615353.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=vubV1HyyC2rPAsIaDdN5CQ' class='article-img'><figcaption><p class='img-caption'>Kẹt xe trên đường Trường Sơn trước cổng sân bay Tân Sơn Nhất, tháng 1/2020. Ảnh: Gia Minh.</figcaption></figure><p class='article-paragraph'>Đường nối Trần Quốc Hoàn - Cộng Hoà được Sở Giao thông Vận tải đề xuất xây dựng từ năm 2016, nhằm giảm ùn tắc quanh sân bay Tân Sơn Nhất nhưng đến nay chưa thể triển khai.Trong khi ga T3 của sân bay khi hoàn thành nâng công suất khai thác mỗi năm lên 50 triệu lượt khách nên công trình được đánh giá rất cấp bách để giảm áp lực giao thông ở khu vực.</p><p class='article-paragraph'>Ngoài dự án nói trên, để giải tỏa kẹt xe cửa ngõ sân bay Tân Sơn Nhất còn<a href='https://vnexpress.net/ba-du-an-cua-ngo-tan-son-nhat-khoi-cong-nam-2021-4218602.html' rel='dofollow'>3 công trình</a> khác dự kiến khởi công năm nay, gồm: mở rộng đường Hoàng Hoa Thám từ cổng doanh trại quân đội (giáp sân bay) đến đường Cộng Hòa(dài gần 800 m, rộng 22 m, vốn đầu tư 257 tỷ đồng); mở rộng một đoạn đường Cộng Hòa, đoạn qua vòng xoay Lăng Cha Cả(dài 134 m, tổng vốn đầu tư 141 tỷ đồng; nâng cấp, mở rộng đường Tân Kỳ Tân Quý, đoạn từ đường Lê Trọng Tấn đến Cộng Hòa(dài hơn 630 m, mở rộng lên 30 m, kinh phí xây dựng gần 110 tỷ đồng).</p><p style = 'text-align:right;' class='article-paragraph'><strong>Gia Minh</strong></p>",
            //        Title = "Kiến nghị sớm mở đường 4.800 tỷ đồng cửa ngõ Tân Sơn Nhất",
            //        ImgUrls = "https://i1-vnexpress.vnecdn.net/2021/03/01/lang-cha-ca-1805-1614615353.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=2tFfrLjYD2sUseqWWdzy0A,https://i1-vnexpress.vnecdn.net/2021/03/01/duong-truong-son-7349-1614615353.jpg?w=680&h=0&q=100&dpr=1&fit=crop&s=vubV1HyyC2rPAsIaDdN5CQ",
            //        Source ="https://vnexpress.net",
            //        CategoryId = 1,
            //        Link = "https://vnexpress.net/kien-nghi-som-mo-duong-4-800-ty-dong-cua-ngo-tan-son-nhat-4242013.html",
            //        Description = "TP HCMChủ đầu tư kiến nghị đẩy nhanh giải phóng mặt bằng để khởi công dự án đường cửa ngõ sân bay Tân Sơn Nhất cuối năm nay, đồng bộ khai thác ga T3 năm 2023.",
            //        CreatedAt = DateTime.Now,
            //         Status =Article.ArticleStatus.ACTIVE
            //    }
            //};
        }
        // GET: Article
        public ActionResult Index()
        {
            var list = GetListArticle();
            var returnList = from a in list select a;

            ViewBag.TopFive = returnList;
            var listCategory = _categories;
            ViewBag.ListCategory = listCategory;
            return View(returnList);
        }

   


        public ActionResult Read(int id) 
        {
            //call api
            var url = ApiEndPoint.GenerateGetArticleByIdUrl(id);
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage runResult = httpClient.GetAsync(url).Result;
                    if (!runResult.IsSuccessStatusCode)
                    {
                        //request failed
                        TempData["AritcleDetailStatus"] = "Get article detais infor failed at index: " + id;
                        return RedirectToAction("ListPendingArticle");
                    }
                    else
                    {
                        //request success
                        var jsonString = runResult.Content.ReadAsStringAsync().Result;
                        var article = JsonConvert.DeserializeObject<Article>(jsonString);
                        ViewBag.listCategory = _categories;
                        ViewBag.listArticle = _articles;
                        return View(article);
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["AritcleDetailStatus"] = $"{err.Message} at index: {id}";
                return null;
            }
            //var article = _articles[id];
            //ViewBag.listCategory = _categories;
            //ViewBag.listArticle = _articles;
            //return View("Article", article);
        }

        private List<Article> GetListArticle()
        {
            var getListAllArticle = ApiEndPoint.ApiDomain + ApiEndPoint.GetListAllArticlePath;

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {


                    HttpResponseMessage runResult = httpClient.GetAsync(getListAllArticle).Result;
                    if (!runResult.IsSuccessStatusCode)
                    {
                        //request failed
                        TempData["getListAllArticle"] = "Get list article Failed!";
                        return null;
                    }
                    else
                    {
                        TempData["GetListPendingArticleStatus"] = "Get list pending article Sucess!";
                        var jsonString = runResult.Content.ReadAsStringAsync().Result;
                        var list = JsonConvert.DeserializeObject<List<Article>>(jsonString);
                        return list.ToList();
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["GetListPendingArticleStatus"] = "Can not connect to API";
                return null;
            }

        }

        private List<Article> SaveArticles()
        {
            return null;
        }
    }
}
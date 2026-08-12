export const common = {
  nav: {
  findDoctor: 'ابحث عن دكتور',
  specialties: 'التخصصات',
  howItWorks: 'كيف يعمل',
  login: 'تسجيل الدخول',
  getStarted: 'ابدأ الآن',
},
  
  buttons: {
    save: 'حفظ',
    cancel: 'إلغاء',
    delete: 'حذف',
    confirm: 'تأكيد',
    back: 'رجوع',
    edit: 'تعديل',
    add: 'إضافة',
    close: 'إغلاق',
    search: 'بحث',
    filter: 'تصفية',
    clear: 'مسح',
    submit: 'إرسال',
    retry: 'حاول مجدداً',
    loadMore: 'تحميل المزيد',
  },

    validation: {
    required: 'هذا الحقل مطلوب',
    invalidEmail: 'يرجى إدخال بريد إلكتروني صحيح',
    invalidPhone: 'يرجى إدخال رقم هاتف صحيح',
    passwordMismatch: 'كلمات المرور غير متطابقة',
    minLength: (n: number) => `الحد الأدنى ${n} أحرف`,
    maxLength: (n: number) => `الحد الأقصى ${n} أحرف`,
    min: (n: number) => `الحد الأدنى للقيمة هو ${n}`,
    max: (n: number) => `الحد الأقصى للقيمة هو ${n}`,
    pattern: 'صيغة غير صحيحة',
    invalidValue: 'قيمة غير صحيحة',
  },

  status: {
    loading: 'جارٍ التحميل...',
    saving: 'جارٍ الحفظ...',
    noData: 'لا توجد بيانات',
    noResults: 'لا توجد نتائج',
    error: 'حدث خطأ ما. يرجى المحاولة مجدداً.',
  },

  pagination: {
    previous: 'السابق',
    next: 'التالي',
    page: (n: number) => `صفحة ${n}`,
    of: 'من',
  },

  confirm: {
    title: 'هل أنت متأكد؟',
    deleteMessage: 'لا يمكن التراجع عن هذا الإجراء.',
    yes: 'نعم، تابع',
    no: 'لا، إلغاء',
  },
  aboutUs: {
    title: 'معلومات عنا',
  },
};

import * as yup from 'yup';

export const RecruitmentPlansValidationSchema = yup.object().shape({
    year: yup.number().min(0).required(),
    plans: yup.array().of(
        yup.object().shape({
            specialityName: yup.string().required(),
            dailyFullBudget: yup.number().min(0).required(),
            dailyFullPaid: yup.number().min(0).required(),
            dailyAbbreviatedBudget: yup.number().min(0).required(),
            dailyAbbreviatedPaid: yup.number().min(0).required(),
            eveningFullBudget: yup.number().min(0).required(),
            eveningFullPaid: yup.number().min(0).required(),
            eveningAbbreviatedBudget: yup.number().min(0).required(),
            eveningAbbreviatedPaid: yup.number().min(0).required(),
        })
    ).required(),
});
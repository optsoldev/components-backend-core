using System;

namespace Optsol.Components.Shared.Settings
{
    public class SwaggerSettings
    {

        public string Title { get; set; }
        public bool Enabled { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public void Validate()
        {
            var titleIsNullOrEmpty = string.IsNullOrEmpty(Title);
            if (titleIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(Title));
            }

            //TODO: Adicionar as outras validações
        }
    }
}

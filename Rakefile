require "rake/clean"

CLEAN.include "*.xam"
CLEAN.include "xamarin-component"

COMPONENT = "Passcode-1.0.2.xam"

file "xamarin-component/xamarin-component.exe" do
	puts "* Downloading xamarin-component..."
	mkdir "xamarin-component"
	sh "curl -L https://components.xamarin.com/submit/xpkg > xamarin-component.zip"
	sh "unzip -o -q xamarin-component.zip -d xamarin-component"
	sh "rm xamarin-component.zip"
end

task :default => "xamarin-component/xamarin-component.exe" do
	line = <<-END
	mono xamarin-component/xamarin-component.exe create-manually #{COMPONENT} \
		--name="Passcode" \
		--summary="Lock screen allowing users to enter a passcode. Can be used right out of the box or you can customize everything. Styled for phone and tablet." \
		--publisher="Kevin Skrei" \
		--website="http://kevinskrei.com" \
		--details="Details.md" \
		--license="License.md" \
		--getting-started="GettingStarted.md" \
		--icon="icons/XamarinPasscode_128x128.png" \
		--icon="icons/XamarinPasscode_512x512.png" \
		--library="android":"XamarinPasscode/Passcode.Google/bin/Release/Passcode.Google.dll" \
		--sample="Passcode Samples. Simple and Customized Samples.":"Samples/XamarinPasscodeSamples/XamarinPasscodeSamples.sln"
		END
	puts "* Creating #{COMPONENT}..."
	puts line.strip.gsub "\t\t", "\\\n    "
	sh line, :verbose => false
	puts "* Created #{COMPONENT}"
end
